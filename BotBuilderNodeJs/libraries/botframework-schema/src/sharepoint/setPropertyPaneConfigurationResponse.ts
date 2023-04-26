// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { ISharepointViewResponse } from "./ISharepointViewResponse";
import { GetCardViewResponse } from "./getCardViewResponse";
import { GetQuickViewResponse } from "./getQuickViewResponse";

/**
 * Sharepoint SetPropertyPaneConfigurationresponse object
 */
export class SetPropertyPaneConfigurationResponse {
    private responseType: SetPropertyPaneConfigurationResponse.ResponseType;
    private renderArguments?: ISharepointViewResponse;

    /**
     * Initializes a new instance of the SetPropertyPaneConfigurationResponse class
     */
    public SetPropertyPaneConfigurationResponse() {
        // Do nothing
    }

    /**
     * Sets response type property of type SetPropertyPaneConfigurationResponse.ResponseType
     */
    public set ReponseType(responseType: SetPropertyPaneConfigurationResponse.ResponseType) {
        this.responseType = responseType;
    }

    /**
     * Gets response type property of type SetPropertyPaneConfigurationResponse.ResponseType
     */
    public get ReponseType(): SetPropertyPaneConfigurationResponse.ResponseType {
        return this.responseType;
    }

    /**
     * Sets render arguments property of type GetCardViewResponse or GetQuickViewResponse
     */
    public set RenderArguments(renderArguments: ISharepointViewResponse) {
        this.renderArguments = renderArguments;
    }

    /**
     * Gets render arguments property of type GetCardViewResponse or GetQuickViewResponse
     */
    public get RenderArguments(): ISharepointViewResponse {
        return this.renderArguments;
    }
}

export namespace SetPropertyPaneConfigurationResponse {
    export enum ResponseType {
        CardView = "Card",
    }
}