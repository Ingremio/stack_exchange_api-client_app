namespace Project.Server.Helpers {
    public class ApiSettings {
        public string? ThisWebApiEndpoint {
            get; set;
        }

        public string? AuthenticationScheme {
            get; set;
        }

        public string[]? AllowedOriginEndpoint {
            get; set;
        }

        public int ClientId {
            get; set;
        }

        public string? ClientSecret {
            get; set;
        }

        public string? Key {
            get; set;
        }

        public string? AuthorizationEndpoint {
            get; set;
        }

        public string? TokenEndpoint {
            get; set;
        }

        public string? UserInformationEndpoint {
            get; set;
        }

        public string? AccessTokenPropertiesEndpoint {
            get; set;
        }

        public string? InvalidateAccessTokenEndpoint {
            get; set;
        }

        public string? TagsEndpoint {
            get; set;
        }
    }
}