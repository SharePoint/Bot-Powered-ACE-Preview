// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { AceData } from './aceData';
import { CardViewData } from './cardViewData';

/**
 * Sharepoint GetCardView response object
 */
export class GetCardViewResponse {
    private templateType: GetCardViewResponse.CardViewTemplateType;
    private aceData: AceData;
    private data: CardViewData;
    private viewId: string;
    /**
     * Initializes a new instance of the GetCardViewResponse class
     */
    public GetCardViewResponse(templateType: GetCardViewResponse.CardViewTemplateType){
        this.TemplateType = templateType;
    }

    /**
     * Sets templateType property of type CardViewTemplateType
     */
    public set TemplateType(templateType: GetCardViewResponse.CardViewTemplateType){
        this.templateType = templateType;
    }

    /**
     * Gets templateType property of type CardViewTemplateType
     */
    public get TemplateType(): GetCardViewResponse.CardViewTemplateType {
        return this.templateType;
    }

    /**
     * Sets aceData property of type AceData
     */
    public set AceData(aceData: AceData){
        this.aceData = aceData;
    }

    /**
     * Gets aceData property of type AceData
     */
    public get AceData(): AceData {
        return this.aceData;
    }

    /**
     * Sets data property of type CardViewData
     */
    public set Data(data: CardViewData){
        this.data = data;
    }

    /**
     * Gets data property of type CardViewData
     */
    public get Data(): CardViewData {
        return this.data;
    }

    /**
     * Sets viewId property of type CardViewData
     */
    public set ViewId(viewId: string){
        this.viewId = viewId;
    }

    /**
     * Gets viewId property of type CardViewData
     */
    public get ViewId(): string {
        return this.viewId;
    }
}

export namespace GetCardViewResponse
{
    export enum CardViewTemplateType {
        PrimaryTextCardView = "PrimaryText",
        ImageCardView = "Image",
        BasicCardView = "Basic"
    }
}