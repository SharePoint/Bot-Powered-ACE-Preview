// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { ISharePointViewResponse } from "./ISharePointViewResponse";

/**
 * Sharepoint SetPropertyPaneConfigurationresponse object
 */
export class SetPropertyPaneConfigurationResponse {
    private responseType: SetPropertyPaneConfigurationResponse.ResponseType;
    private renderArguments?: ISharePointViewResponse;

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
    public set RenderArguments(renderArguments: ISharePointViewResponse) {
        this.renderArguments = renderArguments;
    }

    /**
     * Gets render arguments property of type GetCardViewResponse or GetQuickViewResponse
     */
    public get RenderArguments(): ISharePointViewResponse {
        return this.renderArguments;
    }
}

export namespace SetPropertyPaneConfigurationResponse {
    export enum ResponseType {
        CardView = "Card",
        NoOp = "NoOp"
    }
}