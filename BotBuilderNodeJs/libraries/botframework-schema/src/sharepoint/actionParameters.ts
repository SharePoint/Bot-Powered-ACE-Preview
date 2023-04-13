// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint action button parameters
 */
export class ActionParameters {
    private view: string;
    /**
     * Initializes a new instance of the ActionParameters class
     */
    public ActionParameters(){
        // Do nothing
    }

    /**
     * Sets view property of type string
     */
    public set View(view: string){
        this.view = view;
    }

    /**
     * Gets view property of type string
     */
    public get View(): string {
        return this.view;
    }
}