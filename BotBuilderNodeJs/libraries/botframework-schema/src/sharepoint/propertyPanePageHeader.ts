// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint PropertyPanePageHeader object
 */
export class PropertyPanePageHeader{
    private description: string;
    /**
     * Initializes a new instance of the PropertyPanePageHeader class
     */
    public PropertyPanePageHeader(){
        // Do nothing
    }

    /**
     * Sets the description of type string
     */
    public set Description(description: string){
        this.description = description;
    }

    /**
     * Gets the description of type string
     */
    public get Description(): string {
        return this.description;
    }
}