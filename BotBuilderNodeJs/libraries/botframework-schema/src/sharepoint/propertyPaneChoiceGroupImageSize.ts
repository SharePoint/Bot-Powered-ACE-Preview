// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint PropertyPaneChoiceGroupImageSize object
 */
export class PropertyPaneChoiceGroupImageSize{
    private width: number;
    private height: number;
    /**
     * Initializes a new instance of the PropertyPaneChoiceGroupImageSize class
     */
    public PropertyPaneChoiceGroupImageSize(){
        // Do nothing
    }

    /**
     * Sets the width of the image of type number
     */
    public set Width(width: number){
        this.width = width;
    }

    /**
     * Gets the width of the image of type number
     */
    public get Width(): number {
        return this.width;
    }

    /**
     * Sets the height of the image of type number
     */
    public set Height(height: number){
        this.height = height;
    }

    /**
     * Gets the height of the image of type number
     */
    public get Height(): number {
        return this.height;
    }
}