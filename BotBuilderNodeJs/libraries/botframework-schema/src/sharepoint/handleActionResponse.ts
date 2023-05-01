// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { ISharePointViewResponse } from "./ISharePointViewResponse";

/**
 * Sharepoint HandleActionReponse object
 */
export class HandleActionReponse {
    private responseType: HandleActionReponse.ResponseType;
    private renderArguments?: ISharePointViewResponse;

    /**
     * Initializes a new instance of the HandleActionReponse class
     */
    public HandleActionReponse() {
        // Do nothing
    }

    /**
     * Sets response type property of type HandleActionViewReponse.ResponseType
     */
    public set ReponseType(responseType: HandleActionReponse.ResponseType) {
        this.responseType = responseType;
    }

    /**
     * Gets response type property of type HandleActionViewReponse.ResponseType
     */
    public get ReponseType(): HandleActionReponse.ResponseType {
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

export namespace HandleActionReponse {
    export enum ResponseType {
        CardView = "Card",
        QuickView = "QuickView",
        NoOp = "NoOp"
    }
}