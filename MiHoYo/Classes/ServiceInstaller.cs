﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace MiHoYo_Sharp.Classes
{
    public static class ServiceInstaller
    {
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;

        #region OpenSCManager

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode,
            SetLastError = true)]
        private static extern IntPtr OpenSCManager(string machineName, string databaseName,
            ScmAccessRights dwDesiredAccess);

        #endregion

        #region OpenService

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName,
            ServiceAccessRights dwDesiredAccess);

        #endregion

        #region CreateService

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateService(IntPtr hSCManager, string lpServiceName, string lpDisplayName,
            ServiceAccessRights dwDesiredAccess, int dwServiceType, ServiceBootFlag dwStartType,
            ServiceError dwErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId,
            string lpDependencies, string lp, string lpPassword);

        #endregion

        #region CloseServiceHandle

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseServiceHandle(IntPtr hSCObject);

        #endregion

        #region DeleteService

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteService(IntPtr hService);

        #endregion

        #region ControlService

        [DllImport("advapi32.dll")]
        private static extern int ControlService(IntPtr hService, ServiceControl dwControl,
            SERVICE_STATUS lpServiceStatus);

        #endregion

        #region StartService

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int StartService(IntPtr hService, int dwNumServiceArgs, int lpServiceArgVectors);

        #endregion

        public static void Uninstall(string serviceName)
        {
            var scm = OpenSCManager(ScmAccessRights.AllAccess);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);
                if (service == IntPtr.Zero)
                    return;
                try
                {
                    StopService(service);
                    Thread.Sleep(500);
                    DeleteService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static bool ServiceIsInstalled(string serviceName)
        {
            var scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);

                if (service == IntPtr.Zero)
                    return false;

                CloseServiceHandle(service);
                return true;
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static void InstallAndStart(string serviceName, string displayName, string fileName)
        {
            var scm = OpenSCManager(ScmAccessRights.AllAccess);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.AllAccess);

                if (service == IntPtr.Zero)
                    service = CreateService(scm, serviceName, displayName, ServiceAccessRights.AllAccess, 0x00000001,
                        ServiceBootFlag.AutoStart, ServiceError.Ignore, fileName, null, IntPtr.Zero, null, null, null);

                if (service == IntPtr.Zero)
                    throw new ApplicationException("Failed to install service.");

                try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static void StartService(string serviceName)
        {
            var scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName,
                    ServiceAccessRights.QueryStatus | ServiceAccessRights.Start);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    StartService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        public static void StopService(string serviceName)
        {
            var scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus | ServiceAccessRights.Stop);
                if (service == IntPtr.Zero)
                    throw new ApplicationException("Could not open service.");

                try
                {
                    StopService(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        private static void StartService(IntPtr service)
        {
            var status = new SERVICE_STATUS();
            StartService(service, 0, 0);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StartPending, ServiceState.Running);
            // if (!changedStatus)
            //     throw new ApplicationException("Unable to start service");
        }

        private static void StopService(IntPtr service)
        {
            var status = new SERVICE_STATUS();
            ControlService(service, ServiceControl.Stop, status);
            var changedStatus = WaitForServiceStatus(service, ServiceState.StopPending, ServiceState.Stopped);
            if (!changedStatus)
                return;
        }

        public static ServiceState GetServiceStatus(string serviceName)
        {
            var scm = OpenSCManager(ScmAccessRights.Connect);

            try
            {
                var service = OpenService(scm, serviceName, ServiceAccessRights.QueryStatus);
                if (service == IntPtr.Zero)
                    return ServiceState.NotFound;

                try
                {
                    return GetServiceStatus(service);
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scm);
            }
        }

        private static ServiceState GetServiceStatus(IntPtr service)
        {
            var status = new SERVICE_STATUS();
            status = QueryServiceStatusEx(service);

            if (status == null)
                throw new ApplicationException("Failed to query service status.");

            return status.dwCurrentState;
        }

        private static bool WaitForServiceStatus(IntPtr service, ServiceState waitStatus, ServiceState desiredStatus)
        {
            var status = new SERVICE_STATUS();

            status = QueryServiceStatusEx(service);
            if (status.dwCurrentState == desiredStatus) return true;

            var dwStartTickCount = Environment.TickCount;
            var dwOldCheckPoint = status.dwCheckPoint;

            while (status.dwCurrentState == waitStatus)
            {
                // Do not wait longer than the wait hint. A good interval is
                // one tenth the wait hint, but no less than 1 second and no
                // more than 10 seconds.

                var dwWaitTime = status.dwWaitHint / 10;

                if (dwWaitTime < 1000) dwWaitTime = 1000;
                else if (dwWaitTime > 10000) dwWaitTime = 10000;

                Thread.Sleep(dwWaitTime);

                // Check the status again.
                status = QueryServiceStatusEx(service);
                if (status == null) break;

                if (status.dwCheckPoint > dwOldCheckPoint)
                {
                    // The service is making progress.
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = status.dwCheckPoint;
                }
                else
                {
                    if (Environment.TickCount - dwStartTickCount > status.dwWaitHint)
                        // No progress made within the wait hint
                        break;
                }
            }

            return status.dwCurrentState == desiredStatus;
        }

        private static IntPtr OpenSCManager(ScmAccessRights rights)
        {
            var scm = OpenSCManager(null, null, rights);
            if (scm == IntPtr.Zero)
                throw new ApplicationException("Could not connect to service control manager.");

            return scm;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class SERVICE_STATUS
        {
            public readonly int dwCheckPoint = 0;
            public int dwControlsAccepted = 0;
            public readonly ServiceState dwCurrentState = 0;
            public int dwServiceSpecificExitCode = 0;
            public int dwServiceType = 0;
            public readonly int dwWaitHint = 0;
            public int dwWin32ExitCode = 0;
        }

        #region QueryServiceStatus

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool QueryServiceStatusEx(IntPtr serviceHandle, int infoLevel, IntPtr buffer,
            int bufferSize, out int bytesNeeded);

        private static SERVICE_STATUS QueryServiceStatusEx(IntPtr serviceHandle)
        {
            var buf = IntPtr.Zero;
            try
            {
                var size = 0;

                QueryServiceStatusEx(serviceHandle, 0, buf, size, out size);

                buf = Marshal.AllocHGlobal(size);

                if (!QueryServiceStatusEx(serviceHandle, 0, buf, size, out size))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                return (SERVICE_STATUS) Marshal.PtrToStructure(buf, typeof(SERVICE_STATUS));
            }
            finally
            {
                if (!buf.Equals(IntPtr.Zero))
                    Marshal.FreeHGlobal(buf);
            }
        }


        //[DllImport("advapi32.dll")]
        //private static extern int QueryServiceStatus(IntPtr hService, SERVICE_STATUS lpServiceStatus);

        #endregion
    }


    public enum ServiceState
    {
        Unknown = -1, // The state cannot be (has not been) retrieved.
        NotFound = 0, // The service is not known on the host server.
        Stopped = 1,
        StartPending = 2,
        StopPending = 3,
        Running = 4,
        ContinuePending = 5,
        PausePending = 6,
        Paused = 7
    }

    [Flags]
    public enum ScmAccessRights
    {
        Connect = 0x0001,
        CreateService = 0x0002,
        EnumerateService = 0x0004,
        Lock = 0x0008,
        QueryLockStatus = 0x0010,
        ModifyBootConfig = 0x0020,
        StandardRightsRequired = 0xF0000,

        AllAccess = StandardRightsRequired | Connect | CreateService |
                    EnumerateService | Lock | QueryLockStatus | ModifyBootConfig
    }

    [Flags]
    public enum ServiceAccessRights
    {
        QueryConfig = 0x1,
        ChangeConfig = 0x2,
        QueryStatus = 0x4,
        EnumerateDependants = 0x8,
        Start = 0x10,
        Stop = 0x20,
        PauseContinue = 0x40,
        Interrogate = 0x80,
        UserDefinedControl = 0x100,
        Delete = 0x00010000,
        StandardRightsRequired = 0xF0000,

        AllAccess = StandardRightsRequired | QueryConfig | ChangeConfig |
                    QueryStatus | EnumerateDependants | Start | Stop | PauseContinue |
                    Interrogate | UserDefinedControl
    }

    public enum ServiceBootFlag
    {
        Start = 0x00000000,
        SystemStart = 0x00000001,
        AutoStart = 0x00000002,
        DemandStart = 0x00000003,
        Disabled = 0x00000004
    }

    public enum ServiceControl
    {
        Stop = 0x00000001,
        Pause = 0x00000002,
        Continue = 0x00000003,
        Interrogate = 0x00000004,
        Shutdown = 0x00000005,
        ParamChange = 0x00000006,
        NetBindAdd = 0x00000007,
        NetBindRemove = 0x00000008,
        NetBindEnable = 0x00000009,
        NetBindDisable = 0x0000000A
    }

    public enum ServiceError
    {
        Ignore = 0x00000000,
        Normal = 0x00000001,
        Severe = 0x00000002,
        Critical = 0x00000003
    }
}