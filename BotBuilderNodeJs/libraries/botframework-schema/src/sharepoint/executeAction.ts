// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { Action } from './action';

/**
 * Sharepoint action.execute 
 */
export class ExecuteAction extends Action{
    protected parameters: { [key: string] : unknown };
    private verb: string;

    /**
     * Initializes a new instance of the ExecuteAction class
     */
    public ExecuteAction() {
        // Do nothing
    }

    /**
     * Sets parameters property of type key:value pair
     */
    public set Parameters (parameters: { [key: string] : unknown }) {
        this.parameters = parameters;
    }

    /**
     * Gets parameters property of type key:value pair
     */
    public get Parameters(): { [key: string] : unknown } {
        return this.parameters; 
    }

    /**
     * Sets verb property of type string
     */
    public set Verb (verb: string) {
        this.verb = verb;
    }

    /**
     * Gets verb property of type string
     */
    public get Verb(): string {
        return this.verb; 
    }
}