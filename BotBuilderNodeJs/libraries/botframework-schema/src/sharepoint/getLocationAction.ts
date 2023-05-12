// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { IAction } from './IAction';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';
import { GetLocationActionParameters } from './getLocationActionParameters';

/**
 * Sharepoint external link action
 */
export class GetLocationAction implements IAction, IOnCardSelectionAction {
    private type: string = 'VivaAction.GetLocation';
    protected parameters: GetLocationActionParameters;
    
    /**
     * Initializes a new instance of the GetLocationAction class
     */
    public GetLocationAction() {
        // Do nothing
    }

    /**
     * Sets parameters property of type GetLocationActionParameters
     */
    public set Parameters (parameters: GetLocationActionParameters) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type GetLocationActionParameters
     */
    public get Parameters(): GetLocationActionParameters {
        return this.parameters; 
    }
}