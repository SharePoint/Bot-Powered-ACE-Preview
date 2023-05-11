// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';

/**
 * Sharepoint PropertyPaneLabelProperties object
 */
export class PropertyPaneLabelProperties implements IPropertyPaneFieldProperties {
    private text: string;
    private required: boolean;
    
    /**
     * Initializes a new instance of the PropertyPaneLabelProperties class
     */
    public PropertyPaneLabelProperties() {
        // Do nothing
    }

    /**
     * Sets the label to display next to the checkbox of type string
     */
    public set Text(text: string) {
        this.text = text;
    }

    /**
     * Gets the label to display next to the checkbox of type string
     */
    public get Text(): string {
        return this.text;
    }

    /**
     * Sets a value indicating whether the associated form field is required or not of type boolean
     */
     public set Required(required: boolean) {
        this.required = required;
    }

    /**
     * Gets a value indicating whether the associated form field is required or not of type boolean
     */
    public get Required(): boolean {
        return this.required;
    }

}