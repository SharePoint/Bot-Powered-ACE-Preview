// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IAction } from './IAction';
import { IOnCardSelectionAction } from './IOnCardSelectionAction';
import { ExternalLinkActionParameters } from './externalLinkActionParameters';

/**
 * Sharepoint external link action
 */
export class ExternalLinkAction implements IAction, IOnCardSelectionAction {
    private type: string = 'ExternalLink';
    protected parameters: ExternalLinkActionParameters;
    
    /**
     * Initializes a new instance of the ExternalLinkAction class
     */
    public ExternalLinkAction() {
        // Do nothing
    }

    /**
     * Sets parameters property of type ExternalLinkActionParameters
     */
    public set Parameters (parameters: ExternalLinkActionParameters) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type ExternalLinkActionParameters
     */
    public get Parameters(): ExternalLinkActionParameters {
        return this.parameters; 
    }
}