package com.androidteststudio.helper

import android.accessibilityservice.AccessibilityService
import android.annotation.SuppressLint
import android.app.Notification
import android.content.Intent
import androidx.core.app.NotificationCompat

import android.app.NotificationManager

import android.app.NotificationChannel
import android.content.Context

import android.os.Build
import android.view.WindowManager
import android.view.accessibility.AccessibilityEvent
import android.view.accessibility.AccessibilityNodeInfo
import androidx.annotation.RequiresApi
import java.lang.Exception

class WebServerService : AccessibilityService() {

    private lateinit var _app : WebApp

    override fun onCreate() {
        super.onCreate()
        _app = WebApp(Responder(this, getSystemService(WINDOW_SERVICE) as WindowManager), 10023)
        if (Build.VERSION.SDK_INT >= 26) {
            val CHANNEL_ID = "test_studio"
            val channel = NotificationChannel(
                CHANNEL_ID,
                "Test Studio",
                NotificationManager.IMPORTANCE_MIN
            )
            (getSystemService(Context.NOTIFICATION_SERVICE) as NotificationManager)
                .createNotificationChannel(channel)
            val notification: Notification = NotificationCompat.Builder(this, CHANNEL_ID)
                .setContentTitle("title")
                .setContentText("text").build()
            startForeground(1, notification)
        }
    }

    override fun onStartCommand(intent: Intent?, flags: Int, startId: Int): Int {
        if (!_app.isAlive) _app.start()
        return super.onStartCommand(intent, flags, startId)
    }

    override fun onDestroy() {
        if (_app.isAlive) _app.stop()
    }

    companion object {
        private val changeTypesList = listOf(
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_CONTENT_DESCRIPTION, "CONTENT_CHANGE_TYPE_CONTENT_DESCRIPTION"),
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_STATE_DESCRIPTION, "CONTENT_CHANGE_TYPE_STATE_DESCRIPTION"),
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_SUBTREE, "CONTENT_CHANGE_TYPE_SUBTREE"),
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_TEXT, "CONTENT_CHANGE_TYPE_TEXT"),
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_PANE_TITLE, "CONTENT_CHANGE_TYPE_PANE_TITLE"),
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_UNDEFINED, "CONTENT_CHANGE_TYPE_UNDEFINED"),
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_PANE_APPEARED, "CONTENT_CHANGE_TYPE_PANE_APPEARED"),
                Pair(AccessibilityEvent.CONTENT_CHANGE_TYPE_PANE_DISAPPEARED, "CONTENT_CHANGE_TYPE_PANE_DISAPPEARED"),
            )

        fun getChangeTypesDescription(changeTypes :Int) :String {
            val types = getChangeTypes(changeTypes)
            return types.joinToString(", ")
        }

        fun getChangeTypes(changeTypes :Int) :List<String> {
            val list = mutableListOf<String>()
            for (type in changeTypesList) {
                if (changeTypes.isSet(type.first)) {
                    list.add(type.second)
                }
            }
            return list
        }
    }

    @SuppressLint("NewApi")
    @RequiresApi(Build.VERSION_CODES.KITKAT)
    override fun onAccessibilityEvent(event: AccessibilityEvent?) {
        val time = System.currentTimeMillis()
        try {
            if (null == event) return

            //val window = event?.source ?: return
            val window = windows.first().root

            if (!event.contentChangeTypes.isSet(AccessibilityEvent.CONTENT_CHANGE_TYPE_SUBTREE))
                return

            if (window.flatten<AccessibilityNodeInfo?> { it?.children() }
                .any { it == null }) return

            val views = windows
                .filter { it?.title != null }
                .associateBy(
                {it.title},
                { window -> window?.root?.let { ViewResponse(it)} }
                )
                .filter { it.value != null }

            for (view in views)
                if (view.value.flatten { it?.children ?: emptyList() }.any { null == it })
                    return
            _app.responder.window = ViewChangeResult(
                getChangeTypes(event.contentChangeTypes),
                views as Map<CharSequence?, ViewResponse>,
                time)
        } catch (ex :Exception) {
            ex.toString()
        }
    }

    override fun onInterrupt() {
    }

    override fun onServiceConnected() {
        //windows?.first();
    }

}