<Query Kind="Program">
  <Namespace>System.Net</Namespace>
</Query>

void Main()
{
	var src = new WebClient().DownloadString(@"https://raw.githubusercontent.com/aosp-mirror/platform_frameworks_base/master/core/java/android/view/KeyEvent.java");
	var rgx = new Regex(@"public static final int KEYCODE_(?<Name>[^=]+)[\s]*=[\s]*(?<Value>[0-9]+);");
	foreach (Match m in rgx.Matches(src))
	{
		$"{PascalCase(m.Groups["Name"].Value)} = {m.Groups["Value"].Value},".Dump();
	}
}

// Define other methods and classes here

static string PascalCase(string str)
{
	var parts=str.Split(' ', '_')
		.Where(s => !string.IsNullOrWhiteSpace(s))
		.Select(p => p[0] + new string(p.Skip(1).ToArray()).ToLower());
	var result = string.Join("", parts);
	if (char.IsDigit(result[0]))
		result = '_' + result;
	return result;
}