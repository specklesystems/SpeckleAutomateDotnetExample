using System.Text.Json;

namespace TestAutomateFunction;

public class TestAutomateAppSettings {
    public string? SpeckleToken { get; set; }
    public string? SpeckleServerUrl { get; set; }
    public string? SpeckleProjectId { get; set; }
    public string? SpeckleAutomationId { get; set; }
}

public class TestAutomateEnvironment {

    public static TestAutomateAppSettings? AppSettings { get; private set; }

    private static string GetEnvironmentVariable(string environmentVariableName) {
        var value = TryGetEnvironmentVariable(environmentVariableName);

        if (value is null) {
            throw new Exception($"Cannot run tests without a {environmentVariableName} environment variable");
        }

        return value;
    }

    private static string? TryGetEnvironmentVariable(string environmentVariableName) {
        return Environment.GetEnvironmentVariable(environmentVariableName);
    }

    private static TestAutomateAppSettings? GetAppSettings() {
        if (AppSettings != null) {
            return AppSettings;
        }

        var path = "./appsettings.json";
        var json = File.ReadAllText(path);

        TestAutomateAppSettings appSettings = JsonSerializer.Deserialize<TestAutomateAppSettings>(json);

        AppSettings = appSettings;

        return AppSettings;
    }

    public static string GetSpeckleToken() {
        return GetAppSettings()?.SpeckleToken ?? GetEnvironmentVariable("SPECKLE_TOKEN");
    }

    public static string GetSpeckleServerUrl() {
        return GetAppSettings()?.SpeckleServerUrl ?? TryGetEnvironmentVariable("SPECKLE_SERVER_URL") ?? "http://127.0.0.1:3000";
    }

    public static string GetSpeckleProjectId() {
        return GetAppSettings()?.SpeckleProjectId ?? GetEnvironmentVariable("SPECKLE_PROJECT_ID");
    }

    public static string GetSpeckleAutomationId() {
        return GetAppSettings()?.SpeckleAutomationId ?? GetEnvironmentVariable("SPECKLE_AUTOMATION_ID");
    }

    public static void Clear() {
        AppSettings = null;
    }

}
