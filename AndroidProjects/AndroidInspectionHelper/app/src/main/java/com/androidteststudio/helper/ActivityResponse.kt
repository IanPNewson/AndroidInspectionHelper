package com.androidteststudio.helper

import android.content.pm.ResolveInfo

data class ActivityResponse(val name :String) {

    constructor(activityInfo : ResolveInfo)
        : this(activityInfo.activityInfo.name)

}