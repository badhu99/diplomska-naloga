export interface AuthUserResponse {
    id:                     string;
    accessToken:            string;
    refreshToken:           string;
    refreshTokenExpiration: Date;
    username:               string;
    firstname:              string;
    lastname:               string;
    email:                  string;
}
