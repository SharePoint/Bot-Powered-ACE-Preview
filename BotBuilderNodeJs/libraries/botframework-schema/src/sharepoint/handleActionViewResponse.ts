// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { GetCardViewResponse } from "./getCardViewResponse";
import { GetQuickViewResponse } from "./getQuickViewResponse";

/**
 * Sharepoint HandleActionViewReponse object
 */
export class HandleActionViewReponse {
    private responseType: HandleActionViewReponse.ResponseType;
    private renderArguments?: GetCardViewResponse | GetQuickViewResponse;
    /**
     * Initializes a new instance of the HandleActionViewReponse class
     */
    public HandleActionViewReponse(){
        // Do nothing
    }

    /**
     * Sets response type property of type HandleActionViewReponse.ResponseType
     */
    public set ReponseType(responseType: HandleActionViewReponse.ResponseType){
        this.responseType = responseType;
    }

    /**
     * Gets response type property of type HandleActionViewReponse.ResponseType
     */
    public get ReponseType(): HandleActionViewReponse.ResponseType {
        return this.responseType;
    }

    /**
     * Sets render arguments property of type GetCardViewResponse or GetQuickViewResponse
     */
    public set RenderArguments(renderArguments: GetCardViewResponse | GetQuickViewResponse){
        this.renderArguments = renderArguments;
    }

    /**
     * Gets render arguments property of type GetCardViewResponse or GetQuickViewResponse
     */
    public get RenderArguments(): GetCardViewResponse | GetQuickViewResponse {
        return this.renderArguments;
    }
}

export namespace HandleActionViewReponse{
    export enum ResponseType{
        CardView = "Card",
        QuickView = "QuickView",
        NoOp = "NoOp"
    }
}