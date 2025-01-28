export interface IResult<TResponse> {
  response: TResponse;
  error: Error;
  is_success: boolean;
  status_code: number;
}
