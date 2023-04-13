// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { AceData } from './aceData';
import { CardViewData } from './cardViewData';

export enum CardViewTemplateType {
    PrimaryTextCardView = "PrimaryText",
    ImageCardView = "Image",
    BasicCardView = "Basic"
}

/**
 * Sharepoint GetCardView response object
 */
export class GetCardViewResponse {
    private templateType: CardViewTemplateType;
    private aceData: AceData;
    private data: CardViewData;
    private viewId: string;
    /**
     * Initializes a new instance of the GetCardViewResponse class
     */
    public GetCardViewResponse(templateType: CardViewTemplateType){
        this.TemplateType = templateType;
    }

    /**
     * Sets templateType property of type CardViewTemplateType
     */
    public set TemplateType(templateType: CardViewTemplateType){
        this.templateType = templateType;
    }

    /**
     * Gets templateType property of type CardViewTemplateType
     */
    public get TemplateType(): CardViewTemplateType {
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