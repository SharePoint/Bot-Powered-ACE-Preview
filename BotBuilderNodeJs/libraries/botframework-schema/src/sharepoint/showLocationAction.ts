// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { IAction } from './IAction';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';
import { ShowLocationActionParameters } from './showLocationActionParameters';

/**
 * Sharepoint show location action
 */
export class ShowLocationAction implements IAction, IOnCardSelectionAction{
    private type: string = 'VivaAction.ShowLocation';
    protected parameters: ShowLocationActionParameters;
    
    /**
     * Initializes a new instance of the ShowLocationAction class
     */
    public ShowLocationAction() {
        // Do nothing
    }

    /**
     * Sets parameters property of type ShowLocationActionParameters
     */
    public set Parameters (parameters: ShowLocationActionParameters) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type ShowLocationActionParameters
     */
    public get Parameters(): ShowLocationActionParameters {
        return this.parameters; 
    }
}