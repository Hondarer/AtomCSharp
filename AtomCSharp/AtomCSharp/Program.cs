// from dostools - A collection of command-line utilities by https://github.com/vurdalakov (MIT license)
// https://github.com/vurdalakov/dostools

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AtomCSharp
{
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        #region NT カーネル API 関連

        /// <summary>
        /// NT カーネル API のステータスコードを表します。
        /// </summary>
        public enum NtStatus : uint
        {
            /// <summary>
            /// 操作は正常に終了しました。
            /// </summary>
            Success = 0x00000000,

            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,

            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,

            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,

            /// <summary>
            /// データが大きすぎるため、指定したバッファに格納できません。
            /// </summary>
            BufferOverflow = 0x80000005,

            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,

            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,

            /// <summary>
            /// 指定した情報レコードの長さは、指定した情報クラスに対して必要な長さと一致しません。
            /// </summary>
            InfoLengthMismatch = 0xc0000004,

            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalidHandle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,

            /// <summary>
            /// 要求した操作で必要なオブジェクトの種類と要求に指定したオブジェクトの種類が一致しません。
            /// </summary>
            BufferTooSmall = 0xc0000023,

            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,

            MaximumNtStatus = 0xffffffff
        }

        #endregion

        #region アトム関連

        /// <summary>
        /// グローバルアトムの情報種別を定義します。
        /// </summary>
        public enum AtomInformationClass : int
        {
            AtomBasicInformation,
            AtomTableInformation
        }

        /// <summary>
        /// マーシャリング用のグローバルアトムの基本情報を定義します。
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct ATOM_BASIC_INFORMATION
        {
            /// <summary>
            /// 参照数。
            /// </summary>
            public ushort ReferenceCount;

            /// <summary>
            /// ピン止めか否か。
            /// </summary>
            public ushort Pinned;

            /// <summary>
            /// 名称の長さ。
            /// </summary>
            public ushort NameLength;

            /// <summary>
            /// 名称。
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Name;
        }

        /// <summary>
        /// グローバルアトムの基本情報を取得します。
        /// </summary>
        /// <param name="atom">取得したい文字列を保持しているグローバルアトムを指定します。</param>
        /// <param name="atomInformationClass">グローバルアトムの情報種別。</param>
        /// <param name="atomInformation">グローバルアトムの情報種別の格納先。</param>
        /// <param name="atomInformationLength">グローバルアトムの情報種別の格納先のバッファ長。</param>
        /// <param name="returnLength">格納されたグローバルアトムの情報種別の長さ。</param>
        /// <returns>ステータス情報。</returns>
        [DllImport("ntdll.dll")]
        private static extern NtStatus NtQueryInformationAtom(ushort atom, AtomInformationClass atomInformationClass, IntPtr atomInformation, uint atomInformationLength, out uint returnLength);

        /// <summary>
        /// クリップボードから、指定された登録済みデータ形式の名前を取得します。この関数はその名前を、指定されたバッファへコピーします。
        /// この関数は、ユーザーアトムの名称取得に用いることができます。
        /// </summary>
        /// <param name="format">取得したいデータ形式を指定します。このパラメータは、あらかじめ定義されたデータ形式のいずれかでなければなりません。</param>
        /// <param name="lpszFormatName">データ形式の名前を受け取るバッファへのポインタを指定します。</param>
        /// <param name="cchMaxCount">バッファへコピーされる文字列の最大の長さを文字数単位で指定します。データ形式の名前がこの最大値を上回る場合は、名前が切り捨てられます。</param>
        /// <returns>
        /// 関数が成功すると、バッファへコピーされた文字列の長さが文字数単位で返ります。
        /// 関数が失敗すると、0 が返ります。これは、要求されたデータ形式が存在しないこと、またはあらかじめ定義されたものでないことを示します。
        /// </returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClipboardFormatName(uint format, StringBuilder lpszFormatName, int cchMaxCount);

        /// <summary>
        /// 指定されたローカルアトムに関連付けられている文字列のコピーを取得します。
        /// </summary>
        /// <param name="nAtom">取得したい文字列を保持しているローカルアトムを指定します。</param>
        /// <param name="lpBuffer">文字列を受け取るバッファへのポインタを指定します。</param>
        /// <param name="nSize">バッファのサイズを文字数単位で指定します。</param>
        /// <returns>
        /// 関数が成功すると、バッファにコピーされた文字列の長さが文字数単位で返ります(終端の NULL 文字を除く)。
        /// 関数が失敗すると、0 が返ります。
        /// </returns>
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern ushort GetAtomName(ushort nAtom, StringBuilder lpBuffer, int nSize);

        #endregion

        /// <summary>
        /// アトムの最小値を表します。
        /// </summary>
        const int ATOM_MIN = 0xC000;

        /// <summary>
        /// アトムの最大値を表します。
        /// </summary>
        const int ATOM_MAX = 0xFFFF;

        /// <summary>
        /// グローバルアトムの基本情報を定義します。
        /// </summary>
        public class AtomBasicInformation
        {
            /// <summary>
            /// グローバルアトム。
            /// </summary>
            public ushort Atom { get; private set; }

            /// <summary>
            /// 参照数。
            /// </summary>
            public ushort ReferenceCount { get; private set; }

            /// <summary>
            /// ピン止めか否か。
            /// </summary>
            public ushort Pinned { get; private set; }

            /// <summary>
            /// 名称。
            /// </summary>
            public string Name { get; private set; }

            /// <summary>
            /// <see cref="AtomBasicInformation"/> の新しいインスタンスを初期化します。
            /// </summary>
            /// <param name="atom">グローバルアトム。</param>
            /// <param name="referenceCount">参照数。</param>
            /// <param name="pinned">ピン止めか否か。</param>
            /// <param name="name">名称。</param>
            public AtomBasicInformation(ushort atom, ushort referenceCount, ushort pinned, string name)
            {
                Atom = atom;
                ReferenceCount = referenceCount;
                Pinned = pinned;
                Name = name;
            }
        }

        /// <summary>
        /// グローバルアトムの基本情報を取得します。
        /// </summary>
        /// <param name="atom">取得したい文字列を保持しているグローバルアトムを指定します。</param>
        /// <returns>グローバルアトムの基本情報。取得できない場合は、<c>null</c> を返します。</returns>
        public static AtomBasicInformation NtQueryBasicInformation(ushort atom)
        {
            ATOM_BASIC_INFORMATION atomBasicInformationStruct = new ATOM_BASIC_INFORMATION();

            int size = Marshal.SizeOf(atomBasicInformationStruct);

            IntPtr ptr = Marshal.AllocCoTaskMem(size);

            try
            {
                uint returnLength = 0;
                NtStatus ntstatus = NtQueryInformationAtom(atom, AtomInformationClass.AtomBasicInformation, ptr, (uint)size, out returnLength);
                // NtStatus.InvalidHandle(0xc0000008) is 'not found'
                if (ntstatus != NtStatus.Success)
                {
                    // not found or another error
                    return null;
                }

                atomBasicInformationStruct = (ATOM_BASIC_INFORMATION)Marshal.PtrToStructure(ptr, typeof(ATOM_BASIC_INFORMATION));
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptr);
            }

            return new AtomBasicInformation(atom, atomBasicInformationStruct.ReferenceCount, atomBasicInformationStruct.Pinned, atomBasicInformationStruct.Name);
        }

        /// <summary>
        /// ユーザーアトムの名称を取得します。
        /// </summary>
        /// <param name="atom">取得したい文字列を保持しているユーザーアトムを指定します。</param>
        /// <returns>ユーザーアトムの名称。</returns>
        public static string UserGetName(ushort format)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            int result = GetClipboardFormatName(format, stringBuilder, stringBuilder.Capacity);
            if (result == 0)
            {
                return null;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// ローカルアトムの名称を取得します。
        /// </summary>
        /// <param name="atom">取得したい文字列を保持しているローカルアトムを指定します。</param>
        /// <returns>ローカルアトムの名称。</returns>
        public static string LocalGetName(ushort atom)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            int result = GetAtomName(atom, stringBuilder, stringBuilder.Capacity);
            if (result == 0)
            {
                return null;
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int atom = 0;

            Console.WriteLine("Scope\tAtom\tReferenceCount\tName");

            // Global
            for (atom = ATOM_MIN; atom <= ATOM_MAX; atom++)
            {
                AtomBasicInformation info = NtQueryBasicInformation((ushort)atom);
                if (info != null)
                {
                    Console.WriteLine("Global\t0x{0:x4}\t{1}\t{2}", atom, info.ReferenceCount, info.Name);
                }
            }

            // User
            for (atom = ATOM_MIN; atom <= ATOM_MAX; atom++)
            {
                string name = UserGetName((ushort)atom);
                if (string.IsNullOrEmpty(name) != true)
                {
                    Console.WriteLine("User\t0x{0:x4}\t\t{1}", atom, name);
                }
            }

            // Local
            for (atom = ATOM_MIN; atom <= ATOM_MAX; atom++)
            {
                string name = LocalGetName((ushort)atom);
                if (string.IsNullOrEmpty(name) != true)
                {
                    Console.WriteLine("Local\t0x{0:x4}\t\t{1}", atom, name);
                }
            }
        }
    }
}
