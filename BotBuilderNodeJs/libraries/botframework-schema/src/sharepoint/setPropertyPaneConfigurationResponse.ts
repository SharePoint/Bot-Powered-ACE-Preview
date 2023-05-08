// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { ISharePointViewResponse } from "./ISharePointViewResponse";

/**
 * Sharepoint SetPropertyPaneConfigurationresponse object
 */
export class SetPropertyPaneConfigurationResponse {
    private responseType: SetPropertyPaneConfigurationResponse.ResponseTypeOption;
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
    public set ReponseType(responseType: SetPropertyPaneConfigurationResponse.ResponseTypeOption) {
        this.responseType = responseType;
    }

    /**
     * Gets response type property of type SetPropertyPaneConfigurationResponse.ResponseType
     */
    public get ReponseType(): SetPropertyPaneConfigurationResponse.ResponseTypeOption {
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
    export enum ResponseTypeOption {
        CardView = "Card",
        NoOp = "NoOp"
    }
}