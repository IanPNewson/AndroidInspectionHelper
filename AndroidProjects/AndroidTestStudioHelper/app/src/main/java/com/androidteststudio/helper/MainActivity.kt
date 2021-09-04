package com.androidteststudio.helper

import android.Manifest
import android.annotation.SuppressLint
import android.content.Intent
import android.content.pm.PackageManager
import android.os.Build
import android.os.Bundle
import android.provider.Settings
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.ui.AppBarConfiguration
import androidx.work.*
import com.androidteststudio.helper.databinding.ActivityMainBinding

import android.provider.Settings.SettingNotFoundException
import android.text.TextUtils.SimpleStringSplitter
import android.util.Log
import androidx.activity.result.ActivityResultCallback
import androidx.activity.result.contract.ActivityResultContracts
import androidx.core.content.ContextCompat
import com.androidteststudio.helper.databinding.ContentMainBinding
import com.google.android.material.snackbar.Snackbar


class MainActivity : AppCompatActivity() {

    private val LOGTAG = MainActivity::class.java.`package`.name

    private lateinit var appBarConfiguration: AppBarConfiguration
    private lateinit var binding: ActivityMainBinding
    private lateinit var bindingContent: ContentMainBinding

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        binding = ActivityMainBinding.inflate(layoutInflater)
        setContentView(binding.root)

        setSupportActionBar(binding.toolbar)

        binding.contentMain.btnEnablePermission.click {
            val openSettings = Intent(Settings.ACTION_ACCESSIBILITY_SETTINGS)
            openSettings.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_NO_HISTORY)
            startActivity(openSettings)
        }
    }

    override fun onStart() {
        super.onStart()

        val serviceIntent = Intent(this, WebServerService::class.java)
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            startForegroundService(serviceIntent)
        } else {
            startService(serviceIntent)
        }
    }

    override fun onResume() {
        super.onResume()

        if (isAccessibilityEnabled()) {
            binding.contentMain.containerEnablepermission.hide()
            binding.contentMain.txtMessage.show()
        } else {
            binding.contentMain.containerEnablepermission.show()
            binding.contentMain.txtMessage.hide()
        }

        checkStoragePermission()
    }

    fun checkStoragePermission() {
        storagePermission.launch(Manifest.permission.WRITE_EXTERNAL_STORAGE)
    }

    fun isAccessibilityEnabled(): Boolean {
        var accessibilityEnabled = 0
        val accessibilityFound = false
        try {
            accessibilityEnabled =
                Settings.Secure.getInt(this.contentResolver, Settings.Secure.ACCESSIBILITY_ENABLED)
        } catch (e: SettingNotFoundException) {
            Log.d(LOGTAG, "Error finding setting, default accessibility to not found: " + e.message)
        }
        val mStringColonSplitter = SimpleStringSplitter(':')
        if (accessibilityEnabled == 1) {
            Log.d(LOGTAG, "***ACCESSIBILIY IS ENABLED***: ")
            val settingValue: String = Settings.Secure.getString(
                contentResolver,
                Settings.Secure.ENABLED_ACCESSIBILITY_SERVICES
            )
            Log.d(LOGTAG, "Setting: $settingValue")
            if (settingValue != null) {
                mStringColonSplitter.setString(settingValue)
                while (mStringColonSplitter.hasNext()) {
                    val accessabilityService = mStringColonSplitter.next()
                    if (accessabilityService.equals(
                            WebServerService::class.java.`package`.name + "/" + WebServerService::class.java.canonicalName,
                            ignoreCase = true
                        )
                    ) {
                        return true
                    }
                }
            }
        } else {
            Log.d(LOGTAG, "***ACCESSIBILIY IS DISABLED***")
        }
        return accessibilityFound
    }

    private fun snackBar(text :String) {
        Snackbar.make(binding.root, text, Snackbar.LENGTH_SHORT).show()
    }

    @SuppressLint("NewApi")
    private val storagePermission =
        registerForActivityResult(ActivityResultContracts.RequestPermission()) { granted ->
            with(binding.root) {
                when {
                    granted -> snackBar("Permission granted!")
                    shouldShowRequestPermissionRationale(Manifest.permission.WRITE_EXTERNAL_STORAGE) -> {
                        snackBar("Permission denied, show more info!")
                    }
                    else -> snackBar("Permission denied")
                }
            }
        }

}

