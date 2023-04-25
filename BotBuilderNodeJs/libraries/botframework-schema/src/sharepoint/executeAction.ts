// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { Action } from './action';

/**
 * Sharepoint action.execute 
 */
export class ExecuteAction extends Action{
    private verb: string;
    
    /**
     * Initializes a new instance of the ExecuteAction class
     */
    public ExecuteAction() {
        // Do nothing
    }

    /**
     * Sets verb property of type string
     */
    public set ConfirmationDialog (verb: string) {
        this.verb = verb;
    }

    /**
     * Gets verb property of type string
     */
    public get ConfirmationDialog(): string {
        return this.verb; 
    }
}