// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';

/**
 * Sharepoint PropertyPaneCheckboxProperties object
 */
export class PropertyPaneCheckboxProperties implements IPropertyPaneFieldProperties {
    private text: string
    private disabled: boolean;
    private checked: boolean;
    /**
     * Initializes a new instance of the PropertyPaneCheckboxProperties class
     */
    public PropertyPaneCheckboxProperties(){
        // Do nothing
    }

    /**
     * Sets the label to display next to the checkbox of type string
     */
    public set Text(text: string){
        this.text = text;
    }

    /**
     * Gets the label to display next to the checkbox of type string
     */
    public get Text(): string {
        return this.text;
    }

    /**
     * Sets a value indicating whether this control is enabled or not of type boolean
     */
    public set Disabled(disabled: boolean){
        this.disabled = disabled;
    }

    /**
     * Gets a value indicating whether this control is enabled or not of type boolean
     */
    public get Disabled(): boolean {
        return this.disabled;
    }

    /**
     * Sets a value indicating whether the property pane checkbox is checked or not of type boolean
     */
    public set Checked(checked: boolean){
        this.checked = checked;
    }

    /**
     * Gets a value indicating whether the property pane checkbox is checked or not of type boolean
     */
    public get Checked(): boolean {
        return this.checked;
    }
}