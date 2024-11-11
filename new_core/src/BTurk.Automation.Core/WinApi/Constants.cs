// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
// ReSharper disable GrammarMistakeInComment

namespace BTurk.Automation.Core.WinApi;

public static class Constants
{
    public const int WM_ACTIVATE = 0x0006;
    public const int WM_ACTIVATEAPP = 0x001C;
    public const int WM_AFXFIRST = 0x0360;
    public const int WM_AFXLAST = 0x037F;
    public const int WM_APP = 0x8000;
    public const int WM_ASKCBFORMATNAME = 0x030C;
    public const int WM_CANCELJOURNAL = 0x004B;
    public const int WM_CANCELMODE = 0x001F;
    public const int WM_CAPTURECHANGED = 0x0215;
    public const int WM_CHANGECBCHAIN = 0x030D;
    public const int WM_CHANGEUISTATE = 0x0127;
    public const int WM_CHAR = 0x0102;
    public const int WM_CHARTOITEM = 0x002F;
    public const int WM_CHILDACTIVATE = 0x0022;
    public const int WM_CLEAR = 0x0303;
    public const int WM_CLOSE = 0x0010;
    public const int WM_COMMAND = 0x0111;
    public const int WM_COMPACTING = 0x0041;
    public const int WM_COMPAREITEM = 0x0039;
    public const int WM_CONTEXTMENU = 0x007B;
    public const int WM_COPY = 0x0301;
    public const int WM_COPYDATA = 0x004A;
    public const int WM_CREATE = 0x0001;
    public const int WM_CTLCOLORBTN = 0x0135;
    public const int WM_CTLCOLORDLG = 0x0136;
    public const int WM_CTLCOLOREDIT = 0x0133;
    public const int WM_CTLCOLORLISTBOX = 0x0134;
    public const int WM_CTLCOLORMSGBOX = 0x0132;
    public const int WM_CTLCOLORSCROLLBAR = 0x0137;
    public const int WM_CTLCOLORSTATIC = 0x0138;
    public const int WM_CUT = 0x0300;
    public const int WM_DEADCHAR = 0x0103;
    public const int WM_DELETEITEM = 0x002D;
    public const int WM_DESTROY = 0x0002;
    public const int WM_DESTROYCLIPBOARD = 0x0307;
    public const int WM_DEVICECHANGE = 0x0219;
    public const int WM_DEVMODECHANGE = 0x001B;
    public const int WM_DISPLAYCHANGE = 0x007E;
    public const int WM_DRAWCLIPBOARD = 0x0308;
    public const int WM_DRAWITEM = 0x002B;
    public const int WM_DROPFILES = 0x0233;
    public const int WM_ENABLE = 0x000A;
    public const int WM_ENDSESSION = 0x0016;
    public const int WM_ENTERIDLE = 0x0121;
    public const int WM_ENTERMENULOOP = 0x0211;
    public const int WM_ENTERSIZEMOVE = 0x0231;
    public const int WM_ERASEBKGND = 0x0014;
    public const int WM_EXITMENULOOP = 0x0212;
    public const int WM_EXITSIZEMOVE = 0x0232;
    public const int WM_FONTCHANGE = 0x001D;
    public const int WM_GETDLGCODE = 0x0087;
    public const int WM_GETFONT = 0x0031;
    public const int WM_GETHOTKEY = 0x0033;
    public const int WM_GETICON = 0x007F;
    public const int WM_GETMINMAXINFO = 0x0024;
    public const int WM_GETOBJECT = 0x003D;
    public const int WM_GETTEXT = 0x000D;
    public const int WM_GETTEXTLENGTH = 0x000E;
    public const int WM_HANDHELDFIRST = 0x0358;
    public const int WM_HANDHELDLAST = 0x035F;
    public const int WM_HELP = 0x0053;
    public const int WM_HOTKEY = 0x0312;
    public const int WM_HSCROLL = 0x0114;
    public const int WM_HSCROLLCLIPBOARD = 0x030E;
    public const int WM_ICONERASEBKGND = 0x0027;
    public const int WM_IME_CHAR = 0x0286;
    public const int WM_IME_COMPOSITION = 0x010F;
    public const int WM_IME_COMPOSITIONFULL = 0x0284;
    public const int WM_IME_CONTROL = 0x0283;
    public const int WM_IME_ENDCOMPOSITION = 0x010E;
    public const int WM_IME_KEYDOWN = 0x0290;
    public const int WM_IME_KEYLAST = 0x010F;
    public const int WM_IME_KEYUP = 0x0291;
    public const int WM_IME_NOTIFY = 0x0282;
    public const int WM_IME_REQUEST = 0x0288;
    public const int WM_IME_SELECT = 0x0285;
    public const int WM_IME_SETCONTEXT = 0x0281;
    public const int WM_IME_STARTCOMPOSITION = 0x010D;
    public const int WM_INITDIALOG = 0x0110;
    public const int WM_INITMENU = 0x0116;
    public const int WM_INITMENUPOPUP = 0x0117;
    public const int WM_INPUTLANGCHANGE = 0x0051;
    public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;
    public const int WM_KEYDOWN = 0x0100;
    public const int WM_KEYFIRST = 0x0100;
    public const int WM_KEYLAST = 0x0108;
    public const int WM_KEYUP = 0x0101;
    public const int WM_KILLFOCUS = 0x0008;
    public const int WM_LBUTTONDBLCLK = 0x0203;
    public const int WM_LBUTTONDOWN = 0x0201;
    public const int WM_LBUTTONUP = 0x0202;
    public const int WM_MBUTTONDBLCLK = 0x0209;
    public const int WM_MBUTTONDOWN = 0x0207;
    public const int WM_MBUTTONUP = 0x0208;
    public const int WM_MDIACTIVATE = 0x0222;
    public const int WM_MDICASCADE = 0x0227;
    public const int WM_MDICREATE = 0x0220;
    public const int WM_MDIDESTROY = 0x0221;
    public const int WM_MDIGETACTIVE = 0x0229;
    public const int WM_MDIICONARRANGE = 0x0228;
    public const int WM_MDIMAXIMIZE = 0x0225;
    public const int WM_MDINEXT = 0x0224;
    public const int WM_MDIREFRESHMENU = 0x0234;
    public const int WM_MDIRESTORE = 0x0223;
    public const int WM_MDISETMENU = 0x0230;
    public const int WM_MDITILE = 0x0226;
    public const int WM_MEASUREITEM = 0x002C;
    public const int WM_MENUCHAR = 0x0120;
    public const int WM_MENUCOMMAND = 0x0126;
    public const int WM_MENUDRAG = 0x0123;
    public const int WM_MENUGETOBJECT = 0x0124;
    public const int WM_MENURBUTTONUP = 0x0122;
    public const int WM_MENUSELECT = 0x011F;
    public const int WM_MOUSEACTIVATE = 0x0021;
    public const int WM_MOUSEFIRST = 0x0200;
    public const int WM_MOUSEHOVER = 0x02A1;
    public const int WM_MOUSELAST = 0x020D;
    public const int WM_MOUSELEAVE = 0x02A3;
    public const int WM_MOUSEMOVE = 0x0200;
    public const int WM_MOUSEWHEEL = 0x020A;
    public const int WM_MOUSEHWHEEL = 0x020E;
    public const int WM_MOVE = 0x0003;
    public const int WM_MOVING = 0x0216;
    public const int WM_NCACTIVATE = 0x0086;
    public const int WM_NCCALCSIZE = 0x0083;
    public const int WM_NCCREATE = 0x0081;
    public const int WM_NCDESTROY = 0x0082;
    public const int WM_NCHITTEST = 0x0084;
    public const int WM_NCLBUTTONDBLCLK = 0x00A3;
    public const int WM_NCLBUTTONDOWN = 0x00A1;
    public const int WM_NCLBUTTONUP = 0x00A2;
    public const int WM_NCMBUTTONDBLCLK = 0x00A9;
    public const int WM_NCMBUTTONDOWN = 0x00A7;
    public const int WM_NCMBUTTONUP = 0x00A8;
    public const int WM_NCMOUSEMOVE = 0x00A0;
    public const int WM_NCPAINT = 0x0085;
    public const int WM_NCRBUTTONDBLCLK = 0x00A6;
    public const int WM_NCRBUTTONDOWN = 0x00A4;
    public const int WM_NCRBUTTONUP = 0x00A5;
    public const int WM_NEXTDLGCTL = 0x0028;
    public const int WM_NEXTMENU = 0x0213;
    public const int WM_NOTIFY = 0x004E;
    public const int WM_NOTIFYFORMAT = 0x0055;
    public const int WM_NULL = 0x0000;
    public const int WM_PAINT = 0x000F;
    public const int WM_PAINTCLIPBOARD = 0x0309;
    public const int WM_PAINTICON = 0x0026;
    public const int WM_PALETTECHANGED = 0x0311;
    public const int WM_PALETTEISCHANGING = 0x0310;
    public const int WM_PARENTNOTIFY = 0x0210;
    public const int WM_PASTE = 0x0302;
    public const int WM_PENWINFIRST = 0x0380;
    public const int WM_PENWINLAST = 0x038F;
    public const int WM_POWER = 0x0048;
    public const int WM_POWERBROADCAST = 0x0218;
    public const int WM_PRINT = 0x0317;
    public const int WM_PRINTCLIENT = 0x0318;
    public const int WM_QUERYDRAGICON = 0x0037;
    public const int WM_QUERYENDSESSION = 0x0011;
    public const int WM_QUERYNEWPALETTE = 0x030F;
    public const int WM_QUERYOPEN = 0x0013;
    public const int WM_QUEUESYNC = 0x0023;
    public const int WM_QUIT = 0x0012;
    public const int WM_RBUTTONDBLCLK = 0x0206;
    public const int WM_RBUTTONDOWN = 0x0204;
    public const int WM_RBUTTONUP = 0x0205;
    public const int WM_RENDERALLFORMATS = 0x0306;
    public const int WM_RENDERFORMAT = 0x0305;
    public const int WM_SETCURSOR = 0x0020;
    public const int WM_SETFOCUS = 0x0007;
    public const int WM_SETFONT = 0x0030;
    public const int WM_SETHOTKEY = 0x0032;
    public const int WM_SETICON = 0x0080;
    public const int WM_SETREDRAW = 0x000B;
    public const int WM_SETTEXT = 0x000C;
    public const int WM_SETTINGCHANGE = 0x001A;
    public const int WM_SHOWWINDOW = 0x0018;
    public const int WM_SIZE = 0x0005;
    public const int WM_SIZECLIPBOARD = 0x030B;
    public const int WM_SIZING = 0x0214;
    public const int WM_SPOOLERSTATUS = 0x002A;
    public const int WM_STYLECHANGED = 0x007D;
    public const int WM_STYLECHANGING = 0x007C;
    public const int WM_SYNCPAINT = 0x0088;
    public const int WM_SYSCHAR = 0x0106;
    public const int WM_SYSCOLORCHANGE = 0x0015;
    public const int WM_SYSCOMMAND = 0x0112;
    public const int WM_SYSDEADCHAR = 0x0107;
    public const int WM_SYSKEYDOWN = 0x0104;
    public const int WM_SYSKEYUP = 0x0105;
    public const int WM_TCARD = 0x0052;
    public const int WM_TIMECHANGE = 0x001E;
    public const int WM_TIMER = 0x0113;
    public const int WM_UNDO = 0x0304;
    public const int WM_UNINITMENUPOPUP = 0x0125;
    public const int WM_USER = 0x0400;
    public const int WM_USERCHANGED = 0x0054;
    public const int WM_VKEYTOITEM = 0x002E;
    public const int WM_VSCROLL = 0x0115;
    public const int WM_VSCROLLCLIPBOARD = 0x030A;
    public const int WM_WINDOWPOSCHANGED = 0x0047;
    public const int WM_WINDOWPOSCHANGING = 0x0046;
    public const int WM_WININICHANGE = 0x001A;
    public const int WM_XBUTTONDBLCLK = 0x020D;
    public const int WM_XBUTTONDOWN = 0x020B;
    public const int WM_XBUTTONUP = 0x020C;
    public const int SC_MOVE = 0xF010;

    // WM_SIZE wParam
    public const int SIZE_RESTORED = 0;
    public const int SIZE_MINIMIZED = 1;
    public const int SIZE_MAXIMIZED = 2;
    public const int SIZE_MAXSHOW = 3;
    public const int SIZE_MAXHIDE = 4;


    public const int SWP_ASYNCWINDOWPOS = 0x4000;
    public const int SWP_DEFERERASE = 0x2000;
    public const int SWP_DRAWFRAME = 0x0020;
    public const int SWP_FRAMECHANGED = 0x0020;
    public const int SWP_HIDEWINDOW = 0x0080;
    public const int SWP_NOACTIVATE = 0x0010;
    public const int SWP_NOCOPYBITS = 0x0100;
    public const int SWP_NOMOVE = 0x0002;
    public const int SWP_NOOWNERZORDER = 0x0200;
    public const int SWP_NOREDRAW = 0x0008;
    public const int SWP_NOREPOSITION = 0x0200;
    public const int SWP_NOSENDCHANGING = 0x0400;
    public const int SWP_NOSIZE = 0x0001;
    public const int SWP_NOZORDER = 0x0004;
    public const int SWP_SHOWWINDOW = 0x0040;

    public const int HWND_TOP = 0;
    public const int HWND_BOTTOM = 1;
    public const int HWND_TOPMOST = -1;
    public const int HWND_NOTOPMOST = -2;

    public const int WMSZ_LEFT = 1;
    public const int WMSZ_RIGHT = 2;
    public const int WMSZ_TOP = 3;
    public const int WMSZ_TOPLEFT = 4;
    public const int WMSZ_TOPRIGHT = 5;
    public const int WMSZ_BOTTOM = 6;
    public const int WMSZ_BOTTOMLEFT = 7;
    public const int WMSZ_BOTTOMRIGHT = 8;

    public const int MOD_ALT = 0x0001;

    public const int WH_KEYBOARD_LL = 13;
    public const int HC_ACTION = 0;

    public const int KfAltdown = 0x2000;
    public const int LlkhfAltdown = KfAltdown >> 8;

    public const int VK_LBUTTON = 0x01;
    public const int VK_RBUTTON = 0x02;
    public const int VK_CANCEL = 0x03;
    public const int VK_MBUTTON = 0x04;

    public const int VK_XBUTTON1 = 0x05;
    public const int VK_XBUTTON2 = 0x06;

    public const int VK_BACK = 0x08;
    public const int VK_TAB = 0x09;

    public const int VK_CLEAR = 0x0C;
    public const int VK_RETURN = 0x0D;

    public const int VK_SHIFT = 0x10;
    public const int VK_CONTROL = 0x11;
    public const int VK_MENU = 0x12;
    public const int VK_PAUSE = 0x13;
    public const int VK_CAPITAL = 0x14;

    public const int VK_KANA = 0x15;
    public const int VK_HANGEUL = 0x15;  /* old name - should be here for compatibility */
    public const int VK_HANGUL = 0x15;
    public const int VK_JUNJA = 0x17;
    public const int VK_FINAL = 0x18;
    public const int VK_HANJA = 0x19;
    public const int VK_KANJI = 0x19;

    public const int VK_ESCAPE = 0x1B;

    public const int VK_CONVERT = 0x1C;
    public const int VK_NONCONVERT = 0x1D;
    public const int VK_ACCEPT = 0x1E;
    public const int VK_MODECHANGE = 0x1F;

    public const int VK_SPACE = 0x20;
    public const int VK_PRIOR = 0x21;
    public const int VK_NEXT = 0x22;
    public const int VK_END = 0x23;
    public const int VK_HOME = 0x24;
    public const int VK_LEFT = 0x25;
    public const int VK_UP = 0x26;
    public const int VK_RIGHT = 0x27;
    public const int VK_DOWN = 0x28;
    public const int VK_SELECT = 0x29;
    public const int VK_PRINT = 0x2A;
    public const int VK_EXECUTE = 0x2B;
    public const int VK_SNAPSHOT = 0x2C;
    public const int VK_INSERT = 0x2D;
    public const int VK_DELETE = 0x2E;
    public const int VK_HELP = 0x2F;

    public const int VK_LWIN = 0x5B;
    public const int VK_RWIN = 0x5C;
    public const int VK_APPS = 0x5D;

    public const int VK_SLEEP = 0x5F;

    public const int VK_NUMPAD0 = 0x60;
    public const int VK_NUMPAD1 = 0x61;
    public const int VK_NUMPAD2 = 0x62;
    public const int VK_NUMPAD3 = 0x63;
    public const int VK_NUMPAD4 = 0x64;
    public const int VK_NUMPAD5 = 0x65;
    public const int VK_NUMPAD6 = 0x66;
    public const int VK_NUMPAD7 = 0x67;
    public const int VK_NUMPAD8 = 0x68;
    public const int VK_NUMPAD9 = 0x69;
    public const int VK_MULTIPLY = 0x6A;
    public const int VK_ADD = 0x6B;
    public const int VK_SEPARATOR = 0x6C;
    public const int VK_SUBTRACT = 0x6D;
    public const int VK_DECIMAL = 0x6E;
    public const int VK_DIVIDE = 0x6F;
    public const int VK_F1 = 0x70;
    public const int VK_F2 = 0x71;
    public const int VK_F3 = 0x72;
    public const int VK_F4 = 0x73;
    public const int VK_F5 = 0x74;
    public const int VK_F6 = 0x75;
    public const int VK_F7 = 0x76;
    public const int VK_F8 = 0x77;
    public const int VK_F9 = 0x78;
    public const int VK_F10 = 0x79;
    public const int VK_F11 = 0x7A;
    public const int VK_F12 = 0x7B;
    public const int VK_F13 = 0x7C;
    public const int VK_F14 = 0x7D;
    public const int VK_F15 = 0x7E;
    public const int VK_F16 = 0x7F;
    public const int VK_F17 = 0x80;
    public const int VK_F18 = 0x81;
    public const int VK_F19 = 0x82;
    public const int VK_F20 = 0x83;
    public const int VK_F21 = 0x84;
    public const int VK_F22 = 0x85;
    public const int VK_F23 = 0x86;
    public const int VK_F24 = 0x87;

    public const int VK_NUMLOCK = 0x90;
    public const int VK_SCROLL = 0x91;

    public const int VK_OEM_NEC_EQUAL = 0x92;   // '=' key on numpad

    public const int VK_OEM_FJ_JISHO = 0x92;   // 'Dictionary' key
    public const int VK_OEM_FJ_MASSHOU = 0x93;   // 'Unregister word' key
    public const int VK_OEM_FJ_TOUROKU = 0x94;   // 'Register word' key
    public const int VK_OEM_FJ_LOYA = 0x95;   // 'Left OYAYUBI' key
    public const int VK_OEM_FJ_ROYA = 0x96;   // 'Right OYAYUBI' key

    public const int VK_LSHIFT = 0xA0;
    public const int VK_RSHIFT = 0xA1;
    public const int VK_LCONTROL = 0xA2;
    public const int VK_RCONTROL = 0xA3;
    public const int VK_LMENU = 0xA4;
    public const int VK_RMENU = 0xA5;

    public const int VK_BROWSER_BACK = 0xA6;
    public const int VK_BROWSER_FORWARD = 0xA7;
    public const int VK_BROWSER_REFRESH = 0xA8;
    public const int VK_BROWSER_STOP = 0xA9;
    public const int VK_BROWSER_SEARCH = 0xAA;
    public const int VK_BROWSER_FAVORITES = 0xAB;
    public const int VK_BROWSER_HOME = 0xAC;

    public const int VK_VOLUME_MUTE = 0xAD;
    public const int VK_VOLUME_DOWN = 0xAE;
    public const int VK_VOLUME_UP = 0xAF;
    public const int VK_MEDIA_NEXT_TRACK = 0xB0;
    public const int VK_MEDIA_PREV_TRACK = 0xB1;
    public const int VK_MEDIA_STOP = 0xB2;
    public const int VK_MEDIA_PLAY_PAUSE = 0xB3;
    public const int VK_LAUNCH_MAIL = 0xB4;
    public const int VK_LAUNCH_MEDIA_SELECT = 0xB5;
    public const int VK_LAUNCH_APP1 = 0xB6;
    public const int VK_LAUNCH_APP2 = 0xB7;

    public const int VK_OEM_1 = 0xBA;   // ';:' for US
    public const int VK_OEM_PLUS = 0xBB;   // '+' any country
    public const int VK_OEM_COMMA = 0xBC;   // ',' any country
    public const int VK_OEM_MINUS = 0xBD;   // '-' any country
    public const int VK_OEM_PERIOD = 0xBE;   // '.' any country
    public const int VK_OEM_2 = 0xBF;   // '/?' for US
    public const int VK_OEM_3 = 0xC0;   // '`~' for US

    public const int VK_OEM_4 = 0xDB;  //  '[{' for US
    public const int VK_OEM_5 = 0xDC;  //  '\|' for US
    public const int VK_OEM_6 = 0xDD;  //  ']}' for US
    public const int VK_OEM_7 = 0xDE;  //  ''"' for US
    public const int VK_OEM_8 = 0xDF;

    public const int VK_OEM_AX = 0xE1;  //  'AX' key on Japanese AX kbd
    public const int VK_OEM_102 = 0xE2;  //  "<>" or "\|" on RT 102-key kbd.
    public const int VK_ICO_HELP = 0xE3;  //  Help key on ICO
    public const int VK_ICO_00 = 0xE4;  //  00 key on ICO

    public const int VK_PROCESSKEY = 0xE5;

    public const int VK_ICO_CLEAR = 0xE6;

    public const int VK_PACKET = 0xE7;

    public const int VK_OEM_RESET = 0xE9;
    public const int VK_OEM_JUMP = 0xEA;
    public const int VK_OEM_PA1 = 0xEB;
    public const int VK_OEM_PA2 = 0xEC;
    public const int VK_OEM_PA3 = 0xED;
    public const int VK_OEM_WSCTRL = 0xEE;
    public const int VK_OEM_CUSEL = 0xEF;
    public const int VK_OEM_ATTN = 0xF0;
    public const int VK_OEM_FINISH = 0xF1;
    public const int VK_OEM_COPY = 0xF2;
    public const int VK_OEM_AUTO = 0xF3;
    public const int VK_OEM_ENLW = 0xF4;
    public const int VK_OEM_BACKTAB = 0xF5;

    public const int VK_ATTN = 0xF6;
    public const int VK_CRSEL = 0xF7;
    public const int VK_EXSEL = 0xF8;
    public const int VK_EREOF = 0xF9;
    public const int VK_PLAY = 0xFA;
    public const int VK_ZOOM = 0xFB;
    public const int VK_NONAME = 0xFC;
    public const int VK_PA1 = 0xFD;
    public const int VK_OEM_CLEAR = 0xFE;
}