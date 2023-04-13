// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';

/**
 * Sharepoint PropertyPaneToggleProperties object
 */
export class PropertyPaneToggleProperties implements IPropertyPaneFieldProperties{
    private ariaLabel: string;
    private label: string;
    private disabled: boolean;
    private checked: boolean;
    private key: string;
    private offText: string;
    private onText: string;
    private onAriaLabel: string;
    private offAriaLabel: string;
    /**
     * Initializes a new instance of the PropertyPaneToggleProperties class
     */
    public PropertyPaneToggleProperties(){
        // Do nothing
    }

    /**
     * Sets the aria label of type string
     */
    public set AriaLabel(ariaLabel: string){
        this.ariaLabel = ariaLabel;
    }

    /**
     * Gets the aria label of type string
     */
    public get AriaLabel(): string {
        return this.ariaLabel;
    }

    /**
     * Sets the label of type string
     */
    public set Label(label: string){
        this.label = label;
    }

    /**
     * Gets the label of type string
     */
    public get Label(): string {
        return this.label;
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

    /**
     * Sets a key to uniquely identify the field of type string
     */
    public set Key(key: string){
        this.key = key;
    }

    /**
     * Gets a key to uniquely identify the field of type string
     */
    public get Key(): string {
        return this.key;
    }

    /**
     * Sets text to display when toggle is OFF of type string
     */
    public set OffText(offText: string){
        this.offText = offText;
    }

    /**
     * Gets text to display when toggle is OFF of type string
     */
    public get OffText(): string {
        return this.offText;
    }

    /**
     * Sets text to display when toggle is ON of type string
     */
    public set OnText(onText: string){
        this.onText = onText;
    }

    /**
     * Gets text to display when toggle is ON of type string
     */
    public get OnText(): string {
        return this.onText;
    }

    /**
     * Sets text for screen-reader to announce when toggle is OFF of type string
     */
    public set OffAriaLabel(offAriaLabel: string){
        this.offAriaLabel = offAriaLabel;
    }

    /**
     * Gets text for screen-reader to announce when toggle is OFF of type string
     */
    public get OffAriaLabel(): string {
        return this.offAriaLabel;
    }

    /**
     * Sets text for screen-reader to announce when toggle is ON of type string
     */
    public set OnAriaLabel(onAriaLabel: string){
        this.onAriaLabel = onAriaLabel;
    }

    /**
     * Gets text for screen-reader to announce when toggle is ON of type string
     */
    public get OnAriaLabel(): string {
        return this.onAriaLabel;
    }
}