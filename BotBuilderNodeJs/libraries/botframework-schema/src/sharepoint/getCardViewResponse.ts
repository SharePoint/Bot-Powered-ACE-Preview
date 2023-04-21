// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { AceData } from './aceData';
import { ICardParameters } from './ICardParameters';
import { ICardActionParameters } from './ICardActionParameters';
import { ActionButton } from './actionButton';

/**
 * Sharepoint GetCardView response object
 */
export class GetCardViewResponse {
    private templateType: GetCardViewResponse.CardViewTemplateType;
    private aceData: AceData;
    private data: ICardParameters;
    private viewId: string;
    private onCardSelection: ICardActionParameters;
    private cardButtons: ActionButton;
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
     * Sets data property of type ICardParameters
     */
    public set Data(data: ICardParameters){
        this.data = data;
    }

    /**
     * Gets data property of type ICardParameters
     */
    public get Data(): ICardParameters {
        return this.data;
    }

    /**
     * Sets on card selection property of type ICardActionParameters
     */
    public set OnCardSelection(onCardSelection: ICardActionParameters){
        this.onCardSelection = onCardSelection;
    }

    /**
     * Gets on card selection property of type ICardActionParameters
     */
    public get OnCardSelection(): ICardActionParameters {
        return this.onCardSelection;
    }

    /**
     * Sets card buttons property of type ActionButton
     */
    public set CardButtons(cardButtons: ActionButton){
        this.cardButtons = cardButtons;
    }

    /**
     * Gets card buttons property of type ActionButton
     */
    public get CardButtons(): ActionButton {
        return this.cardButtons;
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
        BasicCardView = "Basic",
        SignInCardView = "SignIn"
    }
}