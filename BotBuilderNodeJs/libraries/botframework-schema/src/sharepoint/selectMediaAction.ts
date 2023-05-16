// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { IAction } from './IAction';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';
import { SelectMediaActionParameters } from './selectMediaActionParameters';

/**
 * Sharepoint select media action
 */
export class SelectMediaAction implements IAction, IOnCardSelectionAction{
    private type: string = 'VivaAction.SelectMedia';
    protected parameters: SelectMediaActionParameters;
    
    /**
     * Initializes a new instance of the SelectMediaAction class
     */
    public SelectMediaAction() {
        // Do nothing
    }

    /**
     * Sets parameters property of type SelectMediaActionParameters
     */
    public set Parameters (parameters: SelectMediaActionParameters) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type SelectMediaActionParameters
     */
    public get Parameters(): SelectMediaActionParameters {
        return this.parameters; 
    }
}