using Android.Lib.Adb.Dumpsys;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AndroidLib.Test
{
    public partial class DumpSysPropertyListParseTest_
    {

        [Fact] public void ParseTest()
        {
            var props = PropertyList.Parse(_SrcSimple, " ");

            Assert.Equal(new[] { "1", "2", "3" }, props["Depth1:"]?["Depth2.Item0:"]?["Depth3.Item0:"].Values);
            Assert.Equal(new[] { "4", "5" }, props["Depth1:"]?["Depth2.Item0:"]?["Depth3.Item1:"].Values);
            Assert.Equal(new[] { "6", "7" }, props["Depth1:"]?["Depth2.Item1:"].Values);
        }

        [Fact] public void FullExampleTest()
        {
            var plist = PropertyList.Parse(_SrcFull, "  ");

            var activities = plist["Activity Resolver Table:"]?["Schemes:"]?["http:"];
        }

        private const string _SrcSimple = 
@"Depth1:
 Depth2.Item0:
  Depth3.Item0:
   1
   2
   3
  Depth3.Item1:
   4
   5
 Depth2.Item1:
  6
  7";

        private const string _SrcFull = @"
Activity Resolver Table:
  Full MIME Types:
      application/vnd.bbc.mobile.weather:
        bd810a6 bbc.mobile.weather/.ui.main.MainActivity filter 40fd53d
          Action: ""android.nfc.action.NDEF_DISCOVERED""
          Category: ""android.intent.category.DEFAULT""
          Category: ""android.intent.category.BROWSABLE""
          StaticType: ""application/vnd.bbc.mobile.weather""

  Base MIME Types:
      application:
        bd810a6 bbc.mobile.weather/.ui.main.MainActivity filter 40fd53d
          Action: ""android.nfc.action.NDEF_DISCOVERED""
          Category: ""android.intent.category.DEFAULT""
          Category: ""android.intent.category.BROWSABLE""
          StaticType: ""application/vnd.bbc.mobile.weather""

  Schemes:
      http:
        bd810a6 bbc.mobile.weather/.ui.main.MainActivity filter e8ac694
          Action: ""android.intent.action.VIEW""
          Category: ""android.intent.category.BROWSABLE""
          Category: ""android.intent.category.DEFAULT""
          Scheme: ""http""
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Authority: ""www.bbc.co.uk"": -1
          Path: ""PatternMatcher{PREFIX: /weather/0}""
          Path: ""PatternMatcher{PREFIX: /weather/1}""
          Path: ""PatternMatcher{PREFIX: /weather/2}""
          Path: ""PatternMatcher{PREFIX: /weather/3}""
          Path: ""PatternMatcher{PREFIX: /weather/4}""
          Path: ""PatternMatcher{PREFIX: /weather/5}""
          Path: ""PatternMatcher{PREFIX: /weather/6}""
          Path: ""PatternMatcher{PREFIX: /weather/7}""
          Path: ""PatternMatcher{PREFIX: /weather/8}""
          Path: ""PatternMatcher{PREFIX: /weather/9}""
          Path: ""PatternMatcher{PREFIX: /weather/A}""
          Path: ""PatternMatcher{PREFIX: /weather/B}""
          Path: ""PatternMatcher{PREFIX: /weather/C}""
          Path: ""PatternMatcher{PREFIX: /weather/D}""
          Path: ""PatternMatcher{PREFIX: /weather/E}""
          Path: ""PatternMatcher{PREFIX: /weather/F}""
          Path: ""PatternMatcher{PREFIX: /weather/G}""
          Path: ""PatternMatcher{PREFIX: /weather/H}""
          Path: ""PatternMatcher{PREFIX: /weather/I}""
          Path: ""PatternMatcher{PREFIX: /weather/J}""
          Path: ""PatternMatcher{PREFIX: /weather/K}""
          Path: ""PatternMatcher{PREFIX: /weather/L}""
          Path: ""PatternMatcher{PREFIX: /weather/M}""
          Path: ""PatternMatcher{PREFIX: /weather/N}""
          Path: ""PatternMatcher{PREFIX: /weather/O}""
          Path: ""PatternMatcher{PREFIX: /weather/P}""
          Path: ""PatternMatcher{PREFIX: /weather/R}""
          Path: ""PatternMatcher{PREFIX: /weather/S}""
          Path: ""PatternMatcher{PREFIX: /weather/T}""
          Path: ""PatternMatcher{PREFIX: /weather/U}""
          Path: ""PatternMatcher{PREFIX: /weather/W}""
          Path: ""PatternMatcher{PREFIX: /weather/Y}""
          Path: ""PatternMatcher{PREFIX: /weather/Z}""
          Path: ""PatternMatcher{PREFIX: /weather/home}""

  Non-Data Actions:
      android.intent.action.MAIN:
        bd810a6 bbc.mobile.weather/.ui.main.MainActivity filter 675dae7
          Action: ""android.intent.action.MAIN""
          Category: ""android.intent.category.LAUNCHER""

  MIME Typed Actions:
      android.nfc.action.NDEF_DISCOVERED:
        bd810a6 bbc.mobile.weather/.ui.main.MainActivity filter 40fd53d
          Action: ""android.nfc.action.NDEF_DISCOVERED""
          Category: ""android.intent.category.DEFAULT""
          Category: ""android.intent.category.BROWSABLE""
          StaticType: ""application/vnd.bbc.mobile.weather""

Receiver Resolver Table:
  Non-Data Actions:
      android.intent.action.QUICKBOOT_POWERON:
        7e49132 bbc.mobile.weather/.Widget filter f3b3e83
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        597b800 bbc.mobile.weather/.Widget_4x1 filter ba98839
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        3c68e7e bbc.mobile.weather/.Widget_2x1 filter e693fdf
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
      android.appwidget.action.APPWIDGET_DELETED:
        7e49132 bbc.mobile.weather/.Widget filter f3b3e83
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        597b800 bbc.mobile.weather/.Widget_4x1 filter ba98839
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        3c68e7e bbc.mobile.weather/.Widget_2x1 filter e693fdf
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
      android.appwidget.action.APPWIDGET_DISABLED:
        7e49132 bbc.mobile.weather/.Widget filter f3b3e83
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        597b800 bbc.mobile.weather/.Widget_4x1 filter ba98839
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        3c68e7e bbc.mobile.weather/.Widget_2x1 filter e693fdf
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
      android.intent.action.BOOT_COMPLETED:
        7e49132 bbc.mobile.weather/.Widget filter f3b3e83
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        597b800 bbc.mobile.weather/.Widget_4x1 filter ba98839
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        3c68e7e bbc.mobile.weather/.Widget_2x1 filter e693fdf
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
      android.appwidget.action.APPWIDGET_ENABLED:
        7e49132 bbc.mobile.weather/.Widget filter f3b3e83
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        597b800 bbc.mobile.weather/.Widget_4x1 filter ba98839
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        3c68e7e bbc.mobile.weather/.Widget_2x1 filter e693fdf
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
      android.appwidget.action.APPWIDGET_UPDATE:
        7e49132 bbc.mobile.weather/.Widget filter f3b3e83
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        597b800 bbc.mobile.weather/.Widget_4x1 filter ba98839
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""
        3c68e7e bbc.mobile.weather/.Widget_2x1 filter e693fdf
          Action: ""android.appwidget.action.APPWIDGET_UPDATE""
          Action: ""android.appwidget.action.APPWIDGET_ENABLED""
          Action: ""android.appwidget.action.APPWIDGET_DELETED""
          Action: ""android.appwidget.action.APPWIDGET_DISABLED""
          Action: ""android.intent.action.BOOT_COMPLETED""
          Action: ""android.intent.action.QUICKBOOT_POWERON""

Service Resolver Table:
  Non-Data Actions:
      android.service.dreams.DreamService:
        9c0542c bbc.mobile.weather/.extension.WeatherDreamExtension filter 6916ef5 permission android.permission.BIND_DREAM_SERVICE
          Action: ""android.service.dreams.DreamService""
          Category: ""android.intent.category.DEFAULT""
      com.google.android.apps.dashclock.Extension:
        239148a bbc.mobile.weather/.extension.WeatherDashClockExtension filter 34bfafb permission com.google.android.apps.dashclock.permission.READ_EXTENSION_DATA
          Action: ""com.google.android.apps.dashclock.Extension""

Registered ContentProviders:
  bbc.mobile.weather/androidx.lifecycle.ProcessLifecycleOwnerInitializer:
    Provider{9f23330 bbc.mobile.weather/androidx.lifecycle.ProcessLifecycleOwnerInitializer}
  bbc.mobile.weather/androidx.core.content.FileProvider:
    Provider{182c261 bbc.mobile.weather/androidx.core.content.FileProvider}

ContentProvider Authorities:
  [bbc.mobile.weather.lifecycle-process]:
    Provider{9f23330 bbc.mobile.weather/androidx.lifecycle.ProcessLifecycleOwnerInitializer}
      applicationInfo=ApplicationInfo{d66f58 bbc.mobile.weather}
  [bbc.mobile.weather.fileprovider]:
    Provider{182c261 bbc.mobile.weather/androidx.core.content.FileProvider}
      applicationInfo=ApplicationInfo{74a68b1 bbc.mobile.weather}

Key Set Manager:
  [bbc.mobile.weather]
      Signing KeySets: 56

Packages:
  Package [bbc.mobile.weather] (da26f48):
    userId=10288
    pkg=Package{5c181e1 bbc.mobile.weather}
    codePath=/data/app/bbc.mobile.weather-z-LmtQhQa6g90GHWMTf7kw==
    resourcePath=/data/app/bbc.mobile.weather-z-LmtQhQa6g90GHWMTf7kw==
    legacyNativeLibraryDir=/data/app/bbc.mobile.weather-z-LmtQhQa6g90GHWMTf7kw==/lib
    primaryCpuAbi=arm64-v8a
    secondaryCpuAbi=null
    versionCode=30328 minSdk=16 targetSdk=29
    versionName=4.0.7
    splits=[base]
    apkSigningVersion=2
    applicationInfo=ApplicationInfo{5c181e1 bbc.mobile.weather}
    flags=[ HAS_CODE ALLOW_CLEAR_USER_DATA ALLOW_BACKUP ]
    privateFlags=[ PRIVATE_FLAG_ACTIVITIES_RESIZE_MODE_UNRESIZEABLE ALLOW_AUDIO_PLAYBACK_CAPTURE HAS_DOMAIN_URLS PRIVATE_FLAG_ALLOW_NATIVE_HEAP_POINTER_TAGGING ]
    forceQueryable=false
    queriesPackages=[]
    dataDir=/data/user/0/bbc.mobile.weather
    supportsScreens=[small, medium, large, xlarge, resizeable, anyDensity]
    usesLibraries:
      android.test.base
    usesOptionalLibraries:
      org.apache.http.legacy
    usesLibraryFiles:
      /system/framework/android.test.base.jar
      /system/framework/org.apache.http.legacy.jar
    timeStamp=2020-06-16 04:20:12
    firstInstallTime=2020-01-04 18:55:47
    lastUpdateTime=2020-06-16 04:20:14
    installerPackageName=com.android.vending
    signatures=PackageSignatures{dfd0b06 version:2, signatures:[2b89aeb], past signatures:[]}
    installPermissionsFixed=true
    pkgFlags=[ HAS_CODE ALLOW_CLEAR_USER_DATA ALLOW_BACKUP ]
    requested permissions:
      android.permission.INTERNET
      android.permission.ACCESS_NETWORK_STATE
      android.permission.ACCESS_FINE_LOCATION
      android.permission.ACCESS_COARSE_LOCATION
      android.permission.ACCESS_BACKGROUND_LOCATION: restricted=true
      android.permission.NFC
      android.permission.RECEIVE_BOOT_COMPLETED
      android.permission.ACCESS_WIFI_STATE
    install permissions:
      android.permission.NFC: granted=true
      android.permission.RECEIVE_BOOT_COMPLETED: granted=true
      android.permission.INTERNET: granted=true
      android.permission.ACCESS_NETWORK_STATE: granted=true
      android.permission.ACCESS_WIFI_STATE: granted=true
    User 0: ceDataInode=3031465 installed=true hidden=false suspended=false distractionFlags=0 stopped=false notLaunched=false enabled=0 instant=false virtual=false
    overlay paths:
      /product/overlay/NavigationBarMode3Button/NavigationBarMode3ButtonOverlay.apk
      /vendor/overlay/oneplus_shape_roundedrect/OnePlusIconShapeRoundedRectOverlay.apk
      gids=[3003]
      runtime permissions:
        android.permission.ACCESS_FINE_LOCATION: granted=true, flags=[ USER_SENSITIVE_WHEN_GRANTED|USER_SENSITIVE_WHEN_DENIED]
        android.permission.ACCESS_COARSE_LOCATION: granted=true, flags=[ USER_SENSITIVE_WHEN_GRANTED|USER_SENSITIVE_WHEN_DENIED]
        android.permission.ACCESS_BACKGROUND_LOCATION: granted=false, flags=[ USER_SENSITIVE_WHEN_GRANTED|USER_SENSITIVE_WHEN_DENIED|RESTRICTION_INSTALLER_EXEMPT|RESTRICTION_UPGRADE_EXEMPT]

Queries:
  system apps queryable: false
  queries via package name:
  queries via intent:
    com.google.android.play.games:
      bbc.mobile.weather
  queryable via interaction:
    User 0:
      [com.oneplus.sound.tuner,com.android.settings,com.oneplus.gamespace,com.android.wallpaperbackup,com.oneplus.factorymode,com.oneplus.coreservice,com.oneplus.orm,com.oneplus.opbackup,com.android.localtransport,com.oneplus.security,com.qti.diagservices,com.dsi.ant.server,com.oem.rftoolkit,com.oem.logkitsdservice,com.android.keychain,com.oneplus.screenshot,com.oneplus.filemanager,com.oneplus.sdcardservice,com.fingerprints.fingerprintsensortest,com.oem.oemlogkit,com.android.server.telecom,com.oneplus,net.oneplus.commonlogtool,com.oneplus.minidumpoptimization,com.oneplus.camera.service,com.android.dynsystem,com.android.location.fused,com.oem.nfc,com.android.providers.settings,com.oneplus.config,com.oneplus.brickmode,net.oneplus.odm,com.tencent.soter.soterserver,com.oneplus.screenrecord,com.android.inputdevices,com.oneplus.setupwizard,android,cn.oneplus.nvbackup,com.oneplus.backuprestore.remoteservice,com.oneplus.opbugreportlite,com.oem.autotest]:
        bbc.mobile.weather
      [com.google.android.gms,com.google.android.gsf]:
        bbc.mobile.weather
      com.google.android.inputmethod.latin:
        bbc.mobile.weather

Package Changes:
  Sequence number=336
  User 0:
    seq=3, package=com.google.android.gms
    seq=19, package=co.faxapp
    seq=39, package=com.google.android.apps.gcs
    seq=92, package=com.redlee90.antlrforandroidpro
    seq=144, package=com.amazon.mShop.android.shopping
    seq=165, package=com.android.vending
    seq=292, package=com.ezviz
    seq=298, package=com.disney.disneyplus
    seq=325, package=com.google.android.apps.chromecast.app
    seq=329, package=com.channel4.ondemand
    seq=333, package=com.google.android.talk
    seq=335, package=com.google.android.apps.docs


Dexopt state:
  [bbc.mobile.weather]
    path: /data/app/bbc.mobile.weather-z-LmtQhQa6g90GHWMTf7kw==/base.apk
      arm64: [status=speed-profile] [reason=bg-dexopt]

mPrimaryDex Enabled
mSecondaryDex Enabled
CPUSET Enabled
ThreadCount Enabled

mBlackList: 3
SCREEN OFF
net.oneplus.launcher
com.google.android.googlequicksearchbox

mSecondaryDexBlackList: 0

mSecondaryDexPkgBlackList: 1
android

mAllDexOptLists: 502
com.google.android.setupwizard
com.oneplus.setupwizard
com.android.mtp
com.google.android.apps.restore
com.google.android.apps.enterprise.dmagent
net.oneplus.launcher
com.oneplus.carrierlocation
com.netflix.mediaclient
com.imdb.mobile
com.quoord.tapatalkpro.activity
com.reddit.frontpage
bbc.mobile.news.uk
com.google.android.apps.podcasts
com.pof.android
com.amazon.avod.thirdpartyclient
com.google.android.deskclock
com.discord
android
com.android.systemui
com.okcupid.okcupid
com.hp.android.printservice
com.hp.printercontrol
com.google.android.apps.docs.editors.docs
com.android.dialer
com.google.android.keep
com.oneplus.camera
com.google.android.play.games
com.eharmony
com.bt.btserviceapp
com.oneplus.gallery
com.Slack
com.google.android.calendar
com.oneplus.mms
com.google.android.apps.magazines
com.ubercab.eats
com.medisafe.android.client
com.google.android.apps.authenticator2
com.sainsburys.gol
com.xda.onehandedmode
com.amazon.mp3
com.frogmind.badland
com.oneplus.contacts
com.scisys.surveillance
com.google.android.apps.translate
com.google.android.videos
com.google.android.ext.services
com.scee.psxandroid
com.playstation.remoteplay
trammanagement.xibis.com.westmidlandsmetro
com.example.testwrongcolours
com.snoggdoggler.android.applications.doggcatcher.premium
com.google.chromeremotedesktop
com.google.stadia.android
com.google.android.music
com.google.android.apps.messaging
com.valvesoftware.android.steam.community
com.anovaculinary.android
de.hafas.android.arriva
com.authy.authy
tv.twitch.android.app
com.stitcher.app
com.google.android.packageinstaller
com.ovelin.guitartuna
com.google.android.apps.nbu.files
com.google.android.apps.docs.editors.sheets
com.android.printspooler
abdelrahman.wifianalyzerpro
com.microsoft.xcloud
com.imgur.mobile
com.entrust.identityGuard.mobile
com.android.keepass
com.trello
com.google.android.inputmethod.latin
com.amazon.dee.app
com.philips.lighting.hue2
com.aftership.AfterShip
com.microsoft.teams
bbc.mobile.weather
com.oneplus.opbackup
com.formagrid.airtable
org.torproject.torbrowser
com.utorrent.client
com.bubblesoft.android.bubbleupnp
org.videolan.vlc
com.google.android.apps.tachyon
com.ocado.mobile.android
com.grppl.android.shell.CMBlloydsTSB73
com.samsung.smartviewad
com.samsung.android.oneconnect
com.audible.application
com.google.android.calculator
com.playstation.mobilemessenger
com.android.settings.intelligence
xyz.klinker.messenger
com.android.captiveportallogin
com.spotify.music
com.appstar.callrecorder
eu.indiewalkabout.fridgemanager
com.google.android.apps.helprtc
com.facebook.orca
com.android.storagemanager
com.google.android.apps.paidtasks
com.grymala.aruler
com.termux
com.shazam.android
uk.co.dominos.android
com.microsoft.rdc.android
com.fragileheart.callrecorder
com.bskyb.nowtv.beta
com.luckybunnyllc.stitchit
com.vitotechnology.StarWalk2Free
com.dah.librarydvd
com.biggu.shopsavvy
com.nintendo.znca
com.google.android.contacts
com.adobe.scan.android
com.google.zxing.client.android
uk.nhs.covid19.production
com.morrisons.mobile.android
com.oneplus.note
com.splendapps.decibel
com.picsart.studio
com.google.android.apps.youtube.music
com.snapchat.android
uk.co.o2.android.myo2
com.oneplus.filemanager
droom.sleepIfUCan
de.medialux.powerftp
com.deliveroo.orderapp
com.vimeo.android.videoapp
com.imray.navigator
com.linkedin.android
com.tuya.smartlife
com.ikvaesolutions.notificationhistorylog
com.sonypicturestelevision.millionaire
com.google.android.apps.fitness
org.zwanoo.android.speedtest
com.oneplus.soundrecorder
uk.co.freeview.android
com.google.android.networkstack.tethering
com.oneplus.calculator
net.oneplus.weather
com.qualcomm.qti.qms.service.telemetry
com.sainsburys.ssa
com.sonymobile.sketch
com.oneplus.gamespace
com.qualcomm.qti.qcolor
hu.szte.ipcg.opencvlivehistogramdemo
com.qualcomm.qti.improvetouch.service
com.android.providers.telephony
com.oneplus.commonoverlay.com.google.android.networkstack
com.android.dynsystem
com.bbc.sounds
org.intoorbit.spectrum
com.google.android.cellbroadcastservice
com.android.providers.calendar
com.huawei.health
com.facebook.mlite
com.oneplus.card
com.android.providers.media
com.FireproofStudios.TheRoom3
com.qti.service.colorservice
com.google.android.onetimeinitializer
com.zynga.words3
com.google.android.ext.shared
iannewson.com.archangel
com.technogym.mywellness
blur.photo.android.app.addquick
com.android.wallpapercropper
org.nativescript.preview
com.oneplus.account
com.oneplus.geoiptime
com.dev47apps.droidcam
com.ubisoft.uplay
de.stefanpledl.localcast
com.thetileapp.tile
com.gofasterstripe.rhlstp.emergencyquestions
com.google.android.trichromelibrary_394511638
cn.oneplus.photos
com.oneplus.wallpaper
com.pluralsight
com.android.externalstorage
com.sensibo.app
com.qualcomm.uimremoteclient
com.android.htmlviewer
com.xibis.videoblogg.videoblogg
com.ghisler.android.TotalCommander
net.oneplus.wallpaperresources
com.qualcomm.qti.uceShimService
com.oneplus.twspods
com.android.companiondevicemanager
com.android.mms.service
com.qualcomm.qti.qms.service.connectionsecurity
com.android.providers.downloads
com.magiumgames.magium
com.qualcomm.qtil.aptxalsOverlay
com.microsoft.xboxone.smartglass
com.oneplus.screenshot
com.kayak.studio.gifmaker
vendor.qti.hardware.cacert.server
com.quicinc.voice.activation
com.microsoft.hockeyapp.testerapp
com.oneplus.brickmode
com.oneplus.deskclock
net.destinydashboard
org.sufficientlysecure.keychain
com.wokInWokOutLtd.WokInWokOutLtd
com.nintendo.znma
com.qualcomm.qti.telephonyservice
com.duolingo
com.qualcomm.qti.performancemode
com.oneplus.sdcardservice
com.oneplus.security
com.oneplus.commonoverlay.com.android.wifi.resources
vendor.qti.iwlan
com.apps.zyinx.mtuchanger.free
com.google.android.configupdater
com.oneplus.backuprestore
com.google.android.providers.media.module
com.futuremark.dmandroid.application
com.rimidalv.dictaphone
cn.oneplus.nvbackup
com.oem.rftoolkit
com.qualcomm.uimremoteserver
com.oneplus.factorymode.specialtest
com.qti.confuridialer
com.zynga.wwf2.free
android.qvaoverlay.common
com.android.systemui.plugin.globalactions.wallet
com.google.ar.core
com.google.ar.lens
com.android.providers.downloads.ui
com.oneplus.wifiapsettings
com.android.pacprocessor
com.android.simappdialog
com.oneplus.iconpack.circle
com.dolby.daxservice
com.fourpixels.aircontrol2
com.volsa.torch
android.overlay.common
net.oneplus.odm
com.playdemic.golf.android
com.oneplus.aodnotification.overlay.purple
com.android.certinstaller
com.android.carrierconfig
com.thetrainline
com.google.android.marvin.talkback
com.oneplus.communication.data
com.oneplus.coreservice
com.oneplus.accessory
com.volcanomobile.drumsequencer
com.qti.qualcomm.datastatusnotification
com.google.android.trichromelibrary.beta_414706983
com.oneplus.applocker
com.hecorat.screenrecorder.free
com.android.hotwordenrollment.xgoogle
com.huawei.bone
com.huawei.hwid
com.amazon.aws.console.mobile
ssw.android.keyboardold
com.qualcomm.qti.callfeaturessetting
com.oneplus.productoverlay.android
com.qualcomm.wfd.service
com.qualcomm.qtil.aptxals
com.playstation.iwyk
com.oneplus.sms.smscplugger
com.paypoint.sparkenergy
com.omgpop.dstfree
com.amazon.now
com.bungieinc.bungiemobile
com.qti.qualcomm.deviceinfo
com.sennheiser.captune
com.oneplus.telephonyoptimization
com.n2elite.manager
com.contextlogic.wish
co.uk.mrwebb.wakeonlan
com.oneplus.factorymode
com.ubnt.easyunifi
com.android.egg
com.android.nfc
com.android.ons
com.android.stk
com.android.backupconfirm
org.linphone
kik.android
com.flavourworks.ericapairing
com.oneplus.iconpack.square
org.codeaurora.ims
com.oneplus.camera.service
com.android.statementservice
com.google.android.as
com.google.android.gm
com.oneplus.commonoverlay.com.oneplus
com.oneplus.commonoverlay.com.android.systemui
android.overlay.target
com.sadpuppy.lemmings
com.google.android.overlay.gmsconfig.common
com.artline.bright.flashlight
com.mobileiq.demand5
net.oneplus.odm.provider
com.oneplus.simcontacts
com.rectfy.cropimage
com.google.android.permissioncontroller
com.qualcomm.qti.dynamicddsservice
net.oneplus.push
com.oneplus.commonoverlay.com.android.networkstack.inprocess.cn
com.mendhak.gpslogger
com.qualcomm.qcrilmsgtunnel
com.android.providers.settings
com.android.sharedstoragebackup
com.oem.nfc
net.oneplus.forums
com.vets4pets
com.oneplus.minidumpoptimization
net.nullsum.freedoom
com.android.hotwordenrollment.okgoogle
com.innersloth.spacemafia
org.torproject.android
com.qualcomm.qti.services.systemhelper
com.android.wifi.resources.overlay.common
org.ifaa.aidl.manager
com.android.dreams.basic
com.bryancandi.android.uituner
io.freetrade.android
com.android.se
com.android.inputdevices
com.google.android.apps.wellbeing
com.oneplus.aodnotification.overlay.red
com.google.android.overlay.gmsconfig.photos
pro.capture.screenshot
com.devlaststudio.policescanner
com.android.bips
com.google.audio.hearing.visualization.accessibility.scribe
com.oneplus.commonoverlay.com.google.android.networkstack.cn
com.qti.dpmserviceapp
com.oneplus.commonoverlay.android
com.playstation.mobile2ndscreen
com.lenovo.anyshare.gps
com.georgie.SoundWire
com.google.android.captiveportallogin
com.bubblesoft.android.bubbleupnp.unlocker
com.google.android.accessibility.soundamplifier
com.ultimateguitar.tonebridge
com.rarlab.rar
com.google.android.apps.maps
com.oneplus.commonoverlay.com.android.networkstack.inprocess
com.jonyups.poziomicagen
com.oneplus.sound.tuner
com.mobile_infographics_tools.mydrive
com.brandiment.cinemapp
com.android.cellbroadcastreceiver
com.microsoft.office.powerpoint
com.oneplus.aodnotification.overlay.gold
com.oem.logkitsdservice
com.qualcomm.qti.simsettings
com.google.android.networkstack
com.google.android.apps.cloudconsole
com.theolivetree.ftpserver
com.android.server.telecom
com.google.android.syncadapters.contacts
com.server.auditor.ssh.client
net.oneplus.widget
com.android.keychain
cn.oneplus.oemtcma
com.android.wifi.resources.overlay.target
org.nativescript.play
com.google.android.gsf
com.google.android.ims
com.google.android.tts
com.oneplus.diagnosemanager
com.android.phone.overlay.common
com.google.android.apps.walletnfcrel
com.android.calllogbackup
com.google.android.partnersetup
com.android.systemui.overlay.common
fr.haruni.frigomagic
com.android.server.telecom.overlay.common
com.android.localtransport
com.google.android.overlay.gmsconfig.gsa
com.android.carrierdefaultapp
com.dsi.ant.server
com.oneplus.iconpack.oneplush2
com.oneplus.iconpack.onepluso2
com.qualcomm.qti.devicestatisticsservice
org.xbmc.kodi
com.google.android.trichromelibrary_410410683
com.android.proxyhandler
com.qualcomm.qti.workloadclassifier
com.oneplus.productoverlay.com.oneplus
com.oneplus.faceunlock
com.oem.oemlogkit
com.byteexperts.texteditor
com.google.android.feedback
com.google.android.printservice.recommendation
com.myklarnamobile
com.android.managedprovisioning
com.oneplus.opbugreportlite
com.google.android.trichromelibrary_428014133
com.android.soundpicker
com.tencent.soter.soterserver
com.google.android.documentsui
com.gamebasic.decibel
org.cohortor.gstrings
com.google.android.apps.unveil
uk.co.esfscorecentre.tigers
com.bt.bms
com.oneplus.screenrecord
com.android.providers.partnerbookmarks
com.oneplus.odmoverlay.com.android.settings
com.havas.petsathome
com.android.wallpaper.livepicker
com.oneplus.productoverlay.com.android.providers.settings
com.oneplus.bttestmode
com.oneplus.backuprestore.remoteservice
net.oneplus.commonlogtool
com.android.apps.tag
com.oneplus.aod
com.oneplus.orm
com.oneplus.ses
com.nand.addtext
com.oneplus.canvasresources
com.amazon.storm.lightning.client.aosp
us.mathlab.android
net.flixster.android
com.google.android.networkstack.permissionconfig
com.android.bookmarkprovider
com.android.settings
com.qualcomm.qti.cne
com.qualcomm.qti.ims
com.qualcomm.qti.lpa
com.qualcomm.qti.smq
com.qualcomm.qti.uim
ltcc.org.freewallet.app
com.haterdater.hater
org.zapto.lp.starlwp
info.guardianproject.orfox
com.oem.autotest
com.google.android.projection.gearhead
com.google.android.apps.giant
com.qualcomm.location
com.google.android.apps.turbo
uk.co.autotrader.androidconsumersearch
com.google.android.wearable.app
com.oneplus.appupgrader
de.hambuch.voronoiapp
com.westernunion.moneytransferr3app.eu
com.oneplus.odmoverlay.android
air.ITVMobilePlayer
com.oneplus.odmoverlay.com.android.systemui
com.qualcomm.qti.uimGbaApp
com.oneplus.odmoverlay.com.oneplus
com.qti.diagservices
com.android.vpndialogs
com.google.android.apps.wallpaper
com.google.android.apps.meetings
deezer.android.app
com.zynga.livepoker
com.android.phone
com.google.vr.vrcore
com.android.shell
com.android.wallpaperbackup
com.android.providers.blockednumber
net.oneplus.provider.appcategoryprovider
com.android.providers.userdictionary
com.android.emergency
com.qualcomm.qti.seccamservice
com.android.hotspot2.osulogin
com.google.android.gms.location.history
com.oneplus.providers.media
com.google.android.inputmethod.japanese
com.android.location.fused
com.android.bluetoothmidiservice
com.qualcomm.qti.poweroffalarm
com.justpark.jp
com.qti.ltebc
com.qualcomm.qti.networksetting
com.android.traceur
com.myprojects.marco.firechat
com.google.android.cellbroadcastreceiver
se.lichtenstein.mind.en
com.google.glass.companion
com.ZumbaRevenge.ZumaDeluxe2017
com.valvesoftware.steamlink
com.qualcomm.qtil.aptxui
net.chaosworship.classicpipes
org.catfantom.multitimerfree
fr.dvilleneuve.lockito
com.android.bluetooth
com.oneplus.config
com.qualcomm.timeservice
com.qualcomm.embms
com.oneplus.dialer
com.android.providers.contacts
si.modula.android.instantheartrate
com.zoiper.android.app
com.media.bestrecorder.audiorecorder
com.android.cellbroadcastreceiver.overlay.common
com.fingerprints.fingerprintsensortest
com.oneplus.engmode
com.djit.apps.stream
com.amazon.kindle
com.google.android.youtube

mWhiteList: 3
com.google.android.setupwizard
com.oneplus.setupwizard
com.android.mtp


Compiler stats:
  [bbc.mobile.weather]
     base.apk - 9360

APEX session state:
  Session ID: 738806733
    State: SUCCESS

Active APEX packages:


Inactive APEX packages:


Factory APEX packages:
";

    }
}
