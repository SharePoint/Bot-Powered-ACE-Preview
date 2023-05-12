// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { IAction } from './IAction';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';
import { QuickViewActionParameters } from './quickViewActionParameters';

/**
 * Sharepoint quick view action
 */
export class QuickViewAction implements IAction, IOnCardSelectionAction {
    private type: string = 'QuickView';
    protected parameters: QuickViewActionParameters;
    
    /**
     * Initializes a new instance of the QuickViewAction class
     */
    public QuickViewAction() {
        // Do nothing
    }

    /**
     * Sets parameters property of type SelectMediaActionParameters
     */
    public set Parameters (parameters: QuickViewActionParameters) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type SelectMediaActionParameters
     */
    public get Parameters(): QuickViewActionParameters {
        return this.parameters; 
    }
}