using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace TravelOrderRecorder
{
	public static class Wlan
	{
		public enum WlanIntfOpcode
		{
			AutoconfEnabled = 1,
			BackgroundScanEnabled,
			MediaStreamingMode,
			RadioState,
			BssType,
			InterfaceState,
			CurrentConnection,
			ChannelNumber,
			SupportedInfrastructureAuthCipherPairs,
			SupportedAdhocAuthCipherPairs,
			SupportedCountryOrRegionStringList,
			CurrentOperationMode,
			Statistics = 0x10000101,
			Rssi,
			SecurityStart = 0x20010000,
			SecurityEnd = 0x2fffffff,
			IhvStart = 0x30000000,
			IhvEnd = 0x3fffffff
		}

		public enum WlanOpcodeValueType
		{
			QueryOnly = 0,
			SetByGroupPolicy = 1,
			SetByUser = 2,
			Invalid = 3
		}

		public const uint WlanClientVersionXpSp2 = 1;
		public const uint WlanClientVersionLonghorn = 2;

		[DllImport("wlanapi.dll")]
		public static extern int WlanOpenHandle(
			[In] UInt32 clientVersion,
			[In, Out] IntPtr pReserved,
			[Out] out UInt32 negotiatedVersion,
			[Out] out IntPtr clientHandle);

		[DllImport("wlanapi.dll")]
		public static extern int WlanCloseHandle(
			[In] IntPtr clientHandle,
			[In, Out] IntPtr pReserved);

		[DllImport("wlanapi.dll")]
		public static extern int WlanEnumInterfaces(
			[In] IntPtr clientHandle,
			[In, Out] IntPtr pReserved,
			[Out] out IntPtr ppInterfaceList);

		[DllImport("wlanapi.dll")]
		public static extern int WlanQueryInterface(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WlanIntfOpcode opCode,
			[In, Out] IntPtr pReserved,
			[Out] out int dataSize,
			[Out] out IntPtr ppData,
			[Out] out WlanOpcodeValueType wlanOpcodeValueType);

		[DllImport("wlanapi.dll")]
		public static extern int WlanSetInterface(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WlanIntfOpcode opCode,
			[In] uint dataSize,
			[In] IntPtr pData,
			[In, Out] IntPtr pReserved);

		[DllImport("wlanapi.dll")]
		public static extern int WlanScan(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] IntPtr pDot11Ssid,
			[In] IntPtr pIeData,
			[In, Out] IntPtr pReserved);

		[Flags]
		public enum WlanGetAvailableNetworkFlags
		{
			IncludeAllAdhocProfiles = 0x00000001,
			IncludeAllManualHiddenProfiles = 0x00000002
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WlanAvailableNetworkListHeader
		{
			public uint numberOfItems;
			public uint index;
		}

		[Flags]
		public enum WlanAvailableNetworkFlags
		{
			Connected = 0x00000001,
			HasProfile = 0x00000002
		}

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
		public struct WlanAvailableNetwork
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string profileName;
			public Dot11Ssid dot11Ssid;
			public Dot11BssType dot11BssType;
			public uint numberOfBssids;
			public bool networkConnectable;
			public WlanReasonCode wlanNotConnectableReason;
			private readonly uint numberOfPhyTypes;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			private readonly Dot11PhyType[] dot11PhyTypes;
			public Dot11PhyType[] Dot11PhyTypes
			{
				get
				{
					Dot11PhyType[] ret = new Dot11PhyType[numberOfPhyTypes];
					Array.Copy(dot11PhyTypes, ret, numberOfPhyTypes);
					return ret;
				}
			}
			public bool morePhyTypes;
			public uint wlanSignalQuality;
			public bool securityEnabled;
			public Dot11AuthAlgorithm dot11DefaultAuthAlgorithm;
			public Dot11CipherAlgorithm dot11DefaultCipherAlgorithm;
			public WlanAvailableNetworkFlags flags;
			readonly uint reserved;
		}

		[DllImport("wlanapi.dll")]
		public static extern int WlanGetAvailableNetworkList(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WlanGetAvailableNetworkFlags flags,
			[In, Out] IntPtr reservedPtr,
			[Out] out IntPtr availableNetworkListPtr);

		[Flags]
		public enum WlanProfileFlags
		{
			AllUser = 0,
			GroupPolicy = 1,
			User = 2
		}

		[DllImport("wlanapi.dll")]
		public static extern int WlanSetProfile(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] WlanProfileFlags flags,
			[In, MarshalAs(UnmanagedType.LPWStr)] string profileXml,
			[In, Optional, MarshalAs(UnmanagedType.LPWStr)] string allUserProfileSecurity,
			[In] bool overwrite,
			[In] IntPtr pReserved,
			[Out] out WlanReasonCode reasonCode);

		[Flags]
		public enum WlanAccess
		{
			ReadAccess = 0x00020000 | 0x0001,
			ExecuteAccess = ReadAccess | 0x0020,
			WriteAccess = ReadAccess | ExecuteAccess | 0x0002 | 0x00010000 | 0x00040000
		}

	    [DllImport("wlanapi.dll")]
		public static extern int WlanGetProfile(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In, MarshalAs(UnmanagedType.LPWStr)] string profileName,
			[In] IntPtr pReserved,
			[Out] out IntPtr profileXml,
			[Out, Optional]  out WlanProfileFlags flags,
			[Out, Optional] out WlanAccess grantedAccess);

		[DllImport("wlanapi.dll")]
		public static extern int WlanGetProfileList(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] IntPtr pReserved,
			[Out] out IntPtr profileList
		);

		[DllImport("wlanapi.dll")]
		public static extern void WlanFreeMemory(IntPtr pMemory);

		[DllImport("wlanapi.dll")]
		public static extern int WlanReasonCodeToString(
			[In] WlanReasonCode reasonCode,
			[In] int bufferSize,
			[In, Out] StringBuilder stringBuffer,
			IntPtr pReserved
		);

		[Flags]
		public enum WlanNotificationSource
		{
			None = 0,
			All = 0X0000FFFF,
			Acm = 0X00000008,
			Msm = 0X00000010,
			Security = 0X00000020,
			Ihv = 0X00000040
		}

		public enum WlanNotificationCodeAcm
		{
			AutoconfEnabled = 1,
			AutoconfDisabled,
			BackgroundScanEnabled,
			BackgroundScanDisabled,
			BssTypeChange,
			PowerSettingChange,
			ScanComplete,
			ScanFail,
			ConnectionStart,
			ConnectionComplete,
			ConnectionAttemptFail,
			FilterListChange,
			InterfaceArrival,
			InterfaceRemoval,
			ProfileChange,
			ProfileNameChange,
			ProfilesExhausted,
			NetworkNotAvailable,
			NetworkAvailable,
			Disconnecting,
			Disconnected,
			AdhocNetworkStateChange
		}

		public enum WlanNotificationCodeMsm
		{
			Associating = 1,
			Associated,
			Authenticating,
			Connected,
			RoamingStart,
			RoamingEnd,
			RadioStateChange,
			SignalQualityChange,
			Disassociating,
			Disconnected,
			PeerJoin,
			PeerLeave,
			AdapterRemoval,
			AdapterOperationModeChange
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WlanNotificationData
		{
			public WlanNotificationSource notificationSource;
			public int notificationCode;
			public Guid interfaceGuid;
			public int dataSize;
			public IntPtr dataPtr;

			public object NotificationCode
			{
				get
				{
					if (notificationSource == WlanNotificationSource.Msm)
						return (WlanNotificationCodeMsm)notificationCode;
					else if (notificationSource == WlanNotificationSource.Acm)
						return (WlanNotificationCodeAcm)notificationCode;
					else
						return notificationCode;
				}

			}
		}

		public delegate void WlanNotificationCallbackDelegate(ref WlanNotificationData notificationData, IntPtr context);

		[DllImport("wlanapi.dll")]
		public static extern int WlanRegisterNotification(
			[In] IntPtr clientHandle,
			[In] WlanNotificationSource notifSource,
			[In] bool ignoreDuplicate,
			[In] WlanNotificationCallbackDelegate funcCallback,
			[In] IntPtr callbackContext,
			[In] IntPtr reserved,
			[Out] out WlanNotificationSource prevNotifSource);

		[Flags]
		public enum WlanConnectionFlags
		{
			HiddenNetwork = 0x00000001,
			AdhocJoinOnly = 0x00000002,
			IgnorePrivacyBit = 0x00000004,
			EapolPassthrough = 0x00000008
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WlanConnectionParameters
		{
			public WlanConnectionMode wlanConnectionMode;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string profile;
			public IntPtr dot11SsidPtr;
			public IntPtr desiredBssidListPtr;
			public Dot11BssType dot11BssType;
			public WlanConnectionFlags flags;
		}

		public enum WlanAdhocNetworkState
		{
			Formed = 0,
			Connected = 1
		}

		[DllImport("wlanapi.dll")]
		public static extern int WlanConnect(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] ref WlanConnectionParameters connectionParameters,
			IntPtr pReserved);

		[DllImport("wlanapi.dll")]
		public static extern int WlanDeleteProfile(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In, MarshalAs(UnmanagedType.LPWStr)] string profileName,
			IntPtr reservedPtr
		);

		[DllImport("wlanapi.dll")]
		public static extern int WlanGetNetworkBssList(
			[In] IntPtr clientHandle,
			[In, MarshalAs(UnmanagedType.LPStruct)] Guid interfaceGuid,
			[In] IntPtr dot11SsidInt,
			[In] Dot11BssType dot11BssType,
			[In] bool securityEnabled,
			IntPtr reservedPtr,
			[Out] out IntPtr wlanBssList
		);

		[StructLayout(LayoutKind.Sequential)]
		internal struct WlanBssListHeader
		{
			internal uint totalSize;
			internal uint numberOfItems;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WlanBssEntry
		{
			public Dot11Ssid dot11Ssid;
			public uint phyId;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public byte[] dot11Bssid;
			public Dot11BssType dot11BssType;
			public Dot11PhyType dot11BssPhyType;
			public int rssi;
			public uint linkQuality;
			public bool inRegDomain;
			public ushort beaconPeriod;
			public ulong timestamp;
			public ulong hostTimestamp;
			public ushort capabilityInformation;
			public uint chCenterFrequency;
			public WlanRateSet wlanRateSet;
			public uint ieOffset;
			public uint ieSize;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WlanRateSet
		{
			private readonly uint rateSetLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 126)]
			private readonly ushort[] rateSet;

			public ushort[] Rates
			{
				get
				{
					ushort[] rates = new ushort[rateSetLength / sizeof(ushort)];
					Array.Copy(rateSet, rates, rates.Length);
					return rates;
				}
			}

			public double GetRateInMbps(int rate)
			{
				return (rateSet[rate] & 0x7FFF) * 0.5;
			}
		} 

		public class WlanException : Exception
		{
			private readonly WlanReasonCode _reasonCode;

			WlanException(WlanReasonCode reasonCode)
			{
				_reasonCode = reasonCode;
			}

			public WlanReasonCode ReasonCode => _reasonCode;

			public override string Message
			{
				get
				{
					StringBuilder sb = new StringBuilder(1024);
					if (WlanReasonCodeToString(_reasonCode, sb.Capacity, sb, IntPtr.Zero) == 0)
						return sb.ToString();
					else
						return string.Empty;
				}
			}
		}

		public enum WlanReasonCode
		{
			Success = 0,
			// general codes
			Unknown = 0x10000 + 1,

			RangeSize = 0x10000,
			Base = 0x10000 + RangeSize,

			// range for Auto Config
			//
			AcBase = 0x10000 + RangeSize,
			AcConnectBase = (AcBase + RangeSize / 2),
			AcEnd = (AcBase + RangeSize - 1),

			// range for profile manager
			// it has profile adding failure reason codes, but may not have 
			// connection reason codes
			//
			ProfileBase = 0x10000 + (7 * RangeSize),
			ProfileConnectBase = (ProfileBase + RangeSize / 2),
			ProfileEnd = (ProfileBase + RangeSize - 1),

			// range for MSM
			//
			MsmBase = 0x10000 + (2 * RangeSize),
			MsmConnectBase = (MsmBase + RangeSize / 2),
			MsmEnd = (MsmBase + RangeSize - 1),

			// range for MSMSEC
			//
			MsmsecBase = 0x10000 + (3 * RangeSize),
			MsmsecConnectBase = (MsmsecBase + RangeSize / 2),
			MsmsecEnd = (MsmsecBase + RangeSize - 1),

			// AC network incompatible reason codes
			//
			NetworkNotCompatible = (AcBase + 1),
			ProfileNotCompatible = (AcBase + 2),

			// AC connect reason code
			//
			NoAutoConnection = (AcConnectBase + 1),
			NotVisible = (AcConnectBase + 2),
			GpDenied = (AcConnectBase + 3),
			UserDenied = (AcConnectBase + 4),
			BssTypeNotAllowed = (AcConnectBase + 5),
			InFailedList = (AcConnectBase + 6),
			InBlockedList = (AcConnectBase + 7),
			SsidListTooLong = (AcConnectBase + 8),
			ConnectCallFail = (AcConnectBase + 9),
			ScanCallFail = (AcConnectBase + 10),
			NetworkNotAvailable = (AcConnectBase + 11),
			ProfileChangedOrDeleted = (AcConnectBase + 12),
			KeyMismatch = (AcConnectBase + 13),
			UserNotRespond = (AcConnectBase + 14),

			// Profile validation errors
			//
			InvalidProfileSchema = (ProfileBase + 1),
			ProfileMissing = (ProfileBase + 2),
			InvalidProfileName = (ProfileBase + 3),
			InvalidProfileType = (ProfileBase + 4),
			InvalidPhyType = (ProfileBase + 5),
			MsmSecurityMissing = (ProfileBase + 6),
			IhvSecurityNotSupported = (ProfileBase + 7),
			IhvOuiMismatch = (ProfileBase + 8),
			// IHV OUI not present but there is IHV settings in profile
			IhvOuiMissing = (ProfileBase + 9),
			// IHV OUI is present but there is no IHV settings in profile
			IhvSettingsMissing = (ProfileBase + 10),
			// both/conflict MSMSec and IHV security settings exist in profile 
			ConflictSecurity = (ProfileBase + 11),
			// no IHV or MSMSec security settings in profile
			SecurityMissing = (ProfileBase + 12),
			InvalidBssType = (ProfileBase + 13),
			InvalidAdhocConnectionMode = (ProfileBase + 14),
			NonBroadcastSetForAdhoc = (ProfileBase + 15),
			AutoSwitchSetForAdhoc = (ProfileBase + 16),
			AutoSwitchSetForManualConnection = (ProfileBase + 17),
			IhvSecurityOnexMissing = (ProfileBase + 18),
			ProfileSsidInvalid = (ProfileBase + 19),
			TooManySsid = (ProfileBase + 20),

			// MSM network incompatible reasons
			//
			UnsupportedSecuritySetByOs = (MsmBase + 1),
			UnsupportedSecuritySet = (MsmBase + 2),
			BssTypeUnmatch = (MsmBase + 3),
			PhyTypeUnmatch = (MsmBase + 4),
			DatarateUnmatch = (MsmBase + 5),

			// MSM connection failure reasons, to be defined
			// failure reason codes
			//
			// user called to disconnect
			UserCancelled = (MsmConnectBase + 1),
			// got disconnect while associating
			AssociationFailure = (MsmConnectBase + 2),
			// timeout for association
			AssociationTimeout = (MsmConnectBase + 3),
			// pre-association security completed with failure
			PreSecurityFailure = (MsmConnectBase + 4),
			// fail to start post-association security
			StartSecurityFailure = (MsmConnectBase + 5),
			// post-association security completed with failure
			SecurityFailure = (MsmConnectBase + 6),
			// security watchdog timeout
			SecurityTimeout = (MsmConnectBase + 7),
			// got disconnect from driver when roaming
			RoamingFailure = (MsmConnectBase + 8),
			// failed to start security for roaming
			RoamingSecurityFailure = (MsmConnectBase + 9),
			// failed to start security for adhoc-join
			AdhocSecurityFailure = (MsmConnectBase + 10),
			// got disconnection from driver
			DriverDisconnected = (MsmConnectBase + 11),
			// driver operation failed
			DriverOperationFailure = (MsmConnectBase + 12),
			// Ihv service is not available
			IhvNotAvailable = (MsmConnectBase + 13),
			// Response from ihv timed out
			IhvNotResponding = (MsmConnectBase + 14),
			// Timed out waiting for driver to disconnect
			DisconnectTimeout = (MsmConnectBase + 15),
			// An internal error prevented the operation from being completed.
			InternalFailure = (MsmConnectBase + 16),
			// UI Request timed out.
			UiRequestTimeout = (MsmConnectBase + 17),
			// Roaming too often, post security is not completed after 5 times.
			TooManySecurityAttempts = (MsmConnectBase + 18),

			// MSMSEC reason codes
			//

			MsmsecMin = MsmsecBase,

			// Key index specified is not valid
			MsmsecProfileInvalidKeyIndex = (MsmsecBase + 1),
			// Key required, PSK present
			MsmsecProfilePskPresent = (MsmsecBase + 2),
			// Invalid key length
			MsmsecProfileKeyLength = (MsmsecBase + 3),
			// Invalid PSK length
			MsmsecProfilePskLength = (MsmsecBase + 4),
			// No auth/cipher specified
			MsmsecProfileNoAuthCipherSpecified = (MsmsecBase + 5),
			// Too many auth/cipher specified
			MsmsecProfileTooManyAuthCipherSpecified = (MsmsecBase + 6),
			// Profile contains duplicate auth/cipher
			MsmsecProfileDuplicateAuthCipher = (MsmsecBase + 7),
			// Profile raw data is invalid (1x or key data)
			MsmsecProfileRawdataInvalid = (MsmsecBase + 8),
			// Invalid auth/cipher combination
			MsmsecProfileInvalidAuthCipher = (MsmsecBase + 9),
			// 802.1x disabled when it's required to be enabled
			MsmsecProfileOnexDisabled = (MsmsecBase + 10),
			// 802.1x enabled when it's required to be disabled
			MsmsecProfileOnexEnabled = (MsmsecBase + 11),
			MsmsecProfileInvalidPmkcacheMode = (MsmsecBase + 12),
			MsmsecProfileInvalidPmkcacheSize = (MsmsecBase + 13),
			MsmsecProfileInvalidPmkcacheTtl = (MsmsecBase + 14),
			MsmsecProfileInvalidPreauthMode = (MsmsecBase + 15),
			MsmsecProfileInvalidPreauthThrottle = (MsmsecBase + 16),
			// PreAuth enabled when PMK cache is disabled
			MsmsecProfilePreauthOnlyEnabled = (MsmsecBase + 17),
			// Capability matching failed at network
			MsmsecCapabilityNetwork = (MsmsecBase + 18),
			// Capability matching failed at NIC
			MsmsecCapabilityNic = (MsmsecBase + 19),
			// Capability matching failed at profile
			MsmsecCapabilityProfile = (MsmsecBase + 20),
			// Network does not support specified discovery type
			MsmsecCapabilityDiscovery = (MsmsecBase + 21),
			// Passphrase contains invalid character
			MsmsecProfilePassphraseChar = (MsmsecBase + 22),
			// Key material contains invalid character
			MsmsecProfileKeymaterialChar = (MsmsecBase + 23),
			// Wrong key type specified for the auth/cipher pair
			MsmsecProfileWrongKeytype = (MsmsecBase + 24),
			// "Mixed cell" suspected (AP not beaconing privacy, we have privacy enabled profile)
			MsmsecMixedCell = (MsmsecBase + 25),
			// Auth timers or number of timeouts in profile is incorrect
			MsmsecProfileAuthTimersInvalid = (MsmsecBase + 26),
			// Group key update interval in profile is incorrect
			MsmsecProfileInvalidGkeyIntv = (MsmsecBase + 27),
			// "Transition network" suspected, trying legacy 802.11 security
			MsmsecTransitionNetwork = (MsmsecBase + 28),
			// Key contains characters which do not map to ASCII
			MsmsecProfileKeyUnmappedChar = (MsmsecBase + 29),
			// Capability matching failed at profile (auth not found)
			MsmsecCapabilityProfileAuth = (MsmsecBase + 30),
			// Capability matching failed at profile (cipher not found)
			MsmsecCapabilityProfileCipher = (MsmsecBase + 31),

			// Failed to queue UI request
			MsmsecUiRequestFailure = (MsmsecConnectBase + 1),
			// 802.1x authentication did not start within configured time 
			MsmsecAuthStartTimeout = (MsmsecConnectBase + 2),
			// 802.1x authentication did not complete within configured time
			MsmsecAuthSuccessTimeout = (MsmsecConnectBase + 3),
			// Dynamic key exchange did not start within configured time
			MsmsecKeyStartTimeout = (MsmsecConnectBase + 4),
			// Dynamic key exchange did not succeed within configured time
			MsmsecKeySuccessTimeout = (MsmsecConnectBase + 5),
			// Message 3 of 4 way handshake has no key data (RSN/WPA)
			MsmsecM3MissingKeyData = (MsmsecConnectBase + 6),
			// Message 3 of 4 way handshake has no IE (RSN/WPA)
			MsmsecM3MissingIe = (MsmsecConnectBase + 7),
			// Message 3 of 4 way handshake has no Group Key (RSN)
			MsmsecM3MissingGrpKey = (MsmsecConnectBase + 8),
			// Matching security capabilities of IE in M3 failed (RSN/WPA)
			MsmsecPrIeMatching = (MsmsecConnectBase + 9),
			// Matching security capabilities of Secondary IE in M3 failed (RSN)
			MsmsecSecIeMatching = (MsmsecConnectBase + 10),
			// Required a pairwise key but AP configured only group keys
			MsmsecNoPairwiseKey = (MsmsecConnectBase + 11),
			// Message 1 of group key handshake has no key data (RSN/WPA)
			MsmsecG1MissingKeyData = (MsmsecConnectBase + 12),
			// Message 1 of group key handshake has no group key
			MsmsecG1MissingGrpKey = (MsmsecConnectBase + 13),
			// AP reset secure bit after connection was secured
			MsmsecPeerIndicatedInsecure = (MsmsecConnectBase + 14),
			// 802.1x indicated there is no authenticator but profile requires 802.1x
			MsmsecNoAuthenticator = (MsmsecConnectBase + 15),
			// Plumbing settings to NIC failed
			MsmsecNicFailure = (MsmsecConnectBase + 16),
			// Operation was cancelled by caller
			MsmsecCancelled = (MsmsecConnectBase + 17),
			// Key was in incorrect format 
			MsmsecKeyFormat = (MsmsecConnectBase + 18),
			// Security downgrade detected
			MsmsecDowngradeDetected = (MsmsecConnectBase + 19),
			// PSK mismatch suspected
			MsmsecPskMismatchSuspected = (MsmsecConnectBase + 20),
			// Forced failure because connection method was not secure
			MsmsecForcedFailure = (MsmsecConnectBase + 21),
			// ui request couldn't be queued or user pressed cancel
			MsmsecSecurityUiFailure = (MsmsecConnectBase + 22),

			MsmsecMax = MsmsecEnd
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WlanConnectionNotificationData
		{
			public WlanConnectionMode wlanConnectionMode;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string profileName;
			public Dot11Ssid dot11Ssid;
			public Dot11BssType dot11BssType;
			public bool securityEnabled;
			public WlanReasonCode wlanReasonCode;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
			public string profileXml;
		}

		public enum WlanInterfaceState
		{
			NotReady = 0,
			Connected = 1,
			AdHocNetworkFormed = 2,
			Disconnecting = 3,
			Disconnected = 4,
			Associating = 5,
			Discovering = 6,
			Authenticating = 7
		}

		public struct Dot11Ssid
		{
			public uint SsidLength;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] Ssid;
		}

		public enum Dot11PhyType : uint
		{
			Unknown = 0,
			Any = Unknown,
			Fhss = 1,
			Dsss = 2,
			IrBaseband = 3,
			Ofdm = 4,
			Hrdsss = 5,
			Erp = 6,
			IhvStart = 0x80000000,
			IhvEnd = 0xffffffff
		}

		public enum Dot11BssType
		{
			Infrastructure = 1,
			Independent = 2,
			Any = 3
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WlanAssociationAttributes
		{
			public Dot11Ssid dot11Ssid;
			public Dot11BssType dot11BssType;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public byte[] dot11Bssid;
			public Dot11PhyType dot11PhyType;
			public uint dot11PhyIndex;
			public uint wlanSignalQuality;
			public uint rxRate;
			public uint txRate;

			public PhysicalAddress Dot11Bssid => new PhysicalAddress(dot11Bssid);
		}

		public enum WlanConnectionMode
		{
			Profile = 0,
			TemporaryProfile,
			DiscoverySecure,
			DiscoveryUnsecure,
			Auto,
			Invalid
		}

		public enum Dot11AuthAlgorithm : uint
		{
			Ieee80211Open = 1,
			Ieee80211SharedKey = 2,
			Wpa = 3,
			WpaPsk = 4,
			WpaNone = 5,
			Rsna = 6,
			RsnaPsk = 7,
			IhvStart = 0x80000000,
			IhvEnd = 0xffffffff
		}

		public enum Dot11CipherAlgorithm : uint
		{
			None = 0x00,
			Wep40 = 0x01,
			Tkip = 0x02,
			Ccmp = 0x04,
			Wep104 = 0x05,
			WpaUseGroup = 0x100,
			RsnUseGroup = 0x100,
			Wep = 0x101,
			IhvStart = 0x80000000,
			IhvEnd = 0xffffffff
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WlanSecurityAttributes
		{
			[MarshalAs(UnmanagedType.Bool)]
			public bool securityEnabled;
			[MarshalAs(UnmanagedType.Bool)]
			public bool oneXEnabled;
			public Dot11AuthAlgorithm dot11AuthAlgorithm;
			public Dot11CipherAlgorithm dot11CipherAlgorithm;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WlanConnectionAttributes
		{
			public WlanInterfaceState isState;
			public WlanConnectionMode wlanConnectionMode;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string profileName;
			public WlanAssociationAttributes wlanAssociationAttributes;
			public WlanSecurityAttributes wlanSecurityAttributes;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WlanInterfaceInfo
		{
			public Guid interfaceGuid;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string interfaceDescription;
			public WlanInterfaceState isState;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WlanInterfaceInfoListHeader
		{
			public uint numberOfItems;
			public uint index;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct WlanProfileInfoListHeader
		{
			public uint numberOfItems;
			public uint index;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct WlanProfileInfo
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string profileName;
			public WlanProfileFlags profileFlags;
		}

		[Flags]
		public enum Dot11OperationMode : uint
		{
			Unknown = 0x00000000,
			Station = 0x00000001,
			Ap = 0x00000002,
			ExtensibleStation = 0x00000004,
			NetworkMonitor = 0x80000000
		}

		[DebuggerStepThrough]
		internal static void ThrowIfError(int win32ErrorCode)
		{
			if (win32ErrorCode != 0)
				throw new Win32Exception(win32ErrorCode);
		}
	}
}
