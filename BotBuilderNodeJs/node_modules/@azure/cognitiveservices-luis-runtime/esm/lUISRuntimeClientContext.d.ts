import * as msRest from "@azure/ms-rest-js";
export declare class LUISRuntimeClientContext extends msRest.ServiceClient {
    endpoint: string;
    credentials: msRest.ServiceClientCredentials;
    /**
     * Initializes a new instance of the LUISRuntimeClientContext class.
     * @param endpoint Supported Cognitive Services endpoints (protocol and hostname, for example:
     * https://westus.api.cognitive.microsoft.com).
     * @param credentials Subscription credentials which uniquely identify client subscription.
     * @param [options] The parameter options
     */
    constructor(credentials: msRest.ServiceClientCredentials, endpoint: string, options?: msRest.ServiceClientOptions);
}
//# sourceMappingURL=lUISRuntimeClientContext.d.ts.map