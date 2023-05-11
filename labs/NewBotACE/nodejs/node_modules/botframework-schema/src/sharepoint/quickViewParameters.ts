// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { ICardActionParameters } from './ICardActionParameters';
import { IOnCardSelectionActionParameters } from './IOnCardSelectionActionParameters';

/**
 * Sharepoint QuickViewParameters object for quick view action
 */
export class QuickViewParameters implements ICardActionParameters, IOnCardSelectionActionParameters {
    private view: string;
    
    /**
     * Initializes a new instance of the QuickViewParameters class
     */
    public QuickViewParameters() {
        // Do nothing
    }

    /**
     * Sets the view id of type string
     */
    public set View(view: string) {
        this.view = view;
    }

    /**
     * Gets the view id of type string
     */
    public get View(): string {
        return this.view;
    }
}