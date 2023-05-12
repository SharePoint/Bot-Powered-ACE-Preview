// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { AceData } from './aceData';
import { ActionButton } from './actionButton';
import { BasicCardParameters } from '.';
import { ICardViewResponse } from './ICardViewResponse';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';

/**
 * Sharepoint BasicCardViewResponse object
 */
export class BasicCardViewResponse implements ICardViewResponse {
    private templateType: string = "Basic";
    private aceData: AceData;
    private data: BasicCardParameters;
    private viewId: string;
    private onCardSelection: IOnCardSelectionAction;
    private cardButtons: ActionButton;

    /**
     * Initializes a new instance of the BasicCardViewResponse class
     */
    public BasicCardViewResponse() {}

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
     * Sets data property of type BasicCardParameters
     */
    public set Data(data: BasicCardParameters) {
        this.data = data;
    }

    /**
     * Gets data property of type BasicCardParameters
     */
    public get Data(): BasicCardParameters {
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
    public set CardButtons(cardButtons: ActionButton) {
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