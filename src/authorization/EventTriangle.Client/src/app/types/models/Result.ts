export interface Result<TResponse> {
  response: TResponse;
  error: Error;
  is_success: boolean;
  status_code: number;
}
