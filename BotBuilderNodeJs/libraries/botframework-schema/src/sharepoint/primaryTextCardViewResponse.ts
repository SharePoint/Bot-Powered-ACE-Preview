// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { AceData } from './aceData';
import { ActionButton } from './actionButton';
import { PrimaryTextCardParameters } from '.';
import { ICardViewResponse } from './ICardViewResponse';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';

/**
 * Sharepoint PrimaryTextCardViewResponse object
 */
export class PrimaryTextCardViewResponse implements ICardViewResponse {
    private templateType: string = "PrimaryText";
    private aceData: AceData;
    private data: PrimaryTextCardParameters;
    private viewId: string;
    private onCardSelection: IOnCardSelectionAction;
    private cardButtons: [ActionButton] | [ActionButton, ActionButton];

    /**
     * Initializes a new instance of the PrimaryTextCardViewResponse class
     */
    public PrimaryTextCardViewResponse() {}

    /**
     * Sets aceData property of type AceData
     */
    public set AceData(aceData: AceData) {
        this.aceData = aceData;
    }

    /**
     * Gets aceData property of type AceData
     */
    public get AceData(): AceData {
        return this.aceData;
    }

    /**
     * Sets data property of type PrimaryTextCardParameters
     */
    public set Data(data: PrimaryTextCardParameters) {
        this.data = data;
    }

    /**
     * Gets data property of type PrimaryTextCardParameters
     */
    public get Data(): PrimaryTextCardParameters {
        return this.data;
    }

    /**
     * Sets on card selection property of type IOnCardSelectionAction
     */
    public set OnCardSelection(onCardSelection: IOnCardSelectionAction) {
        this.onCardSelection = onCardSelection;
    }

    /**
     * Gets on card selection property of type IOnCardSelectionAction
     */
    public get OnCardSelection(): IOnCardSelectionAction {
        return this.onCardSelection;
    }

    /**
     * Sets card buttons property of type ActionButton
     */
    public set CardButtons(cardButtons: [ActionButton] | [ActionButton, ActionButton]) {
        if (cardButtons.length > 2) {
            this.cardButtons = cardButtons.splice(0,2) as [ActionButton];
        } else {
            this.cardButtons = cardButtons;
        }
    }

    /**
     * Gets card buttons property of type ActionButton
     */
    public get CardButtons(): [ActionButton] | [ActionButton, ActionButton] {
        return this.cardButtons;
    }

    /**
     * Sets viewId property of type CardViewData
     */
    public set ViewId(viewId: string) {
        this.viewId = viewId;
    }

    /**
     * Gets viewId property of type CardViewData
     */
    public get ViewId(): string {
        return this.viewId;
    }
}