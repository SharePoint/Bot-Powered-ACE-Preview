// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint QuickViewActionParameters object for quick view action
 */
export class QuickViewActionParameters {
    private view: string;
    
    /**
     * Initializes a new instance of the QuickViewActionParameters class
     */
    public QuickViewActionParameters() {
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