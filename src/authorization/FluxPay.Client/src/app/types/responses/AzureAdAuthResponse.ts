export interface AzureAdAuthResponse {
  token_type: string;
  scope: string;
  expires_in: string;
  ext_expires_in: string;
  access_token: string;
  refresh_token: string;
  id_token: string;
}
