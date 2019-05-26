package md5d3d094d5c54a924dc170cf36586ac58f;


public class AppWidget
	extends android.appwidget.AppWidgetProvider
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onEnabled:(Landroid/content/Context;)V:GetOnEnabled_Landroid_content_Context_Handler\n" +
			"n_onUpdate:(Landroid/content/Context;Landroid/appwidget/AppWidgetManager;[I)V:GetOnUpdate_Landroid_content_Context_Landroid_appwidget_AppWidgetManager_arrayIHandler\n" +
			"n_onDeleted:(Landroid/content/Context;[I)V:GetOnDeleted_Landroid_content_Context_arrayIHandler\n" +
			"n_onDisabled:(Landroid/content/Context;)V:GetOnDisabled_Landroid_content_Context_Handler\n" +
			"";
		mono.android.Runtime.register ("Lab_4.AppWidget, Lab_4", AppWidget.class, __md_methods);
	}


	public AppWidget ()
	{
		super ();
		if (getClass () == AppWidget.class)
			mono.android.TypeManager.Activate ("Lab_4.AppWidget, Lab_4", "", this, new java.lang.Object[] {  });
	}


	public void onEnabled (android.content.Context p0)
	{
		n_onEnabled (p0);
	}

	private native void n_onEnabled (android.content.Context p0);


	public void onUpdate (android.content.Context p0, android.appwidget.AppWidgetManager p1, int[] p2)
	{
		n_onUpdate (p0, p1, p2);
	}

	private native void n_onUpdate (android.content.Context p0, android.appwidget.AppWidgetManager p1, int[] p2);


	public void onDeleted (android.content.Context p0, int[] p1)
	{
		n_onDeleted (p0, p1);
	}

	private native void n_onDeleted (android.content.Context p0, int[] p1);


	public void onDisabled (android.content.Context p0)
	{
		n_onDisabled (p0);
	}

	private native void n_onDisabled (android.content.Context p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
