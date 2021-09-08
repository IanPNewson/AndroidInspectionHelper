package com.androidteststudio.helper

import android.content.pm.PackageInfo
import android.content.pm.PackageManager

data class PackageResponse(val packageId :String, val name :String, val version :String?) {

    var defaultActivity : ActivityResponse? = null

    constructor(pkg: PackageInfo, pm :PackageManager, defaultActivity :ActivityResponse? = null) :
        this(pkg.packageName,
        pm.getApplicationLabel(pkg.applicationInfo).toString(),
        pkg.versionName) {
            this.defaultActivity = defaultActivity
        }
}

