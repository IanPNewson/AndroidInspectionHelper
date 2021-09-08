package com.androidteststudio.helper

import android.content.Context
import android.content.pm.PackageManager
import android.graphics.Bitmap
import android.graphics.Canvas
import android.graphics.Color
import android.graphics.drawable.BitmapDrawable
import android.graphics.drawable.Drawable
import android.content.Intent
import android.view.WindowManager


class Responder(val ctx: Context, val windowManager: WindowManager) {

    var window : ViewChangeResult? = null

    fun getPackageList(): List<PackageResponse> {
        val pm = ctx.packageManager

        val mainIntent = Intent(Intent.ACTION_MAIN, null)
            .addCategory(Intent.CATEGORY_LAUNCHER)

        val defaultActivities = pm.queryIntentActivities(mainIntent, 0)

        val pkgs = pm.getInstalledPackages(PackageManager.GET_ACTIVITIES).mapNotNull { pkg ->
            val defaultActivity =
                defaultActivities.firstOrNull { act -> act.activityInfo.packageName == pkg.packageName }
            defaultActivity?.activityInfo?.name?.let {
                PackageResponse(pkg, pm, ActivityResponse(defaultActivity)) }
        }
        return pkgs
    }

    fun getPackageIcon(packageId: String): Bitmap? {
        val pm = ctx.packageManager
        val icon = pm.getApplicationIcon(packageId)
        return toBitmap(icon)
    }

    private fun toBitmap(drawable :Drawable) : Bitmap {
        if (drawable is BitmapDrawable) return drawable.bitmap
        val bmp = Bitmap.createBitmap(drawable.intrinsicWidth, drawable.intrinsicHeight, Bitmap.Config.ARGB_8888);
        bmp.eraseColor(Color.TRANSPARENT)
        val canvas = Canvas(bmp);
        drawable.setBounds(0, 0, canvas.width, canvas.height);
        drawable.draw(canvas);
        return bmp;
    }

    fun getViews(): ViewChangeResult? {
        return window
    }

}

