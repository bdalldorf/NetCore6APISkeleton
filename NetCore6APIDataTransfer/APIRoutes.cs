namespace NetCore6APIDataTransfer
{
    public static class ApiRoutes
    {
        public static class ApiStartupRoute
        {
            public const string GetApiStartup = "api/start";
        }

        public static class WebAuthenticationRoute
        {
            public const string PostWebLoginInformation = "authenticate/web/login";
            public const string PostWebLoginAntiForgeryCookie = "authenticate/web/antiforgerycookie";
            public const string PostWebLogoutInformation = "authenticate/web/logout";
        }

        public static class APIAuthenticationRoute
        {
            public const string PostAPILoginInformation = "authenticate/api/login";
        }

        public static class APITestRoute
        {
            public const string GetApiTestItems = "apitest/getall/";
            public const string GetApiTestItem = "apitest/get/{id}";
            public const string SaveApiTestItem = "apitest/save";
            public const string UpdateApiTestItem = "apitest/update";
            public const string DeleteApiTestItem = "apitest/delete";
        }
    }
}
