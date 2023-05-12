// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { AceData } from './aceData';
import { ActionButton } from './actionButton';
import { ImageCardParameters } from '.';
import { ICardViewResponse } from './ICardViewResponse';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';

/**
 * Sharepoint ImageCardViewResponse object
 */
export class ImageCardViewResponse implements ICardViewResponse {
    private templateType: string = "Image";
    private aceData: AceData;
    private data: ImageCardParameters;
    private viewId: string;
    private onCardSelection: IOnCardSelectionAction;
    private cardButtons: ActionButton;

    /**
     * Initializes a new instance of the ImageCardViewResponse class
     */
    public ImageCardViewResponse() {}

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
     * Sets data property of type ImageCardParameters
     */
    public set Data(data: ImageCardParameters) {
        this.data = data;
    }

    /**
     * Gets data property of type ImageCardParameters
     */
    public get Data(): ImageCardParameters {
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