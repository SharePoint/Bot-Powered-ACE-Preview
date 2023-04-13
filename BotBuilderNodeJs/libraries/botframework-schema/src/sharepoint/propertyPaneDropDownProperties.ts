// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';
import { PropertyPaneDropDownOption } from './propertyPaneDropDownOptions';

/**
 * Sharepoint PropertyPaneDropDownProperties object
 */
export class PropertyPaneDropDownProperties implements IPropertyPaneFieldProperties {
    private ariaLabel: string;
    private ariaPositionInSet: number;
    private ariaSetSize: number;
    private label: string;
    private disabled: boolean;
    private errorMessage: string;
    private selectedKey: string;
    private options: [PropertyPaneDropDownOption];
    /**
     * Initializes a new instance of the PropertyPaneDropDownProperties class
     */
    public PropertyPaneDropDownProperties(){
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
     * Sets an element's number or position in the current set of controls.
     * Maps to native aria-posinset attribute. It starts from 1 of type number
     */
    public set AriaPositionInSet(ariaPositionInSet: number){
        this.ariaPositionInSet = ariaPositionInSet;
    }

    /**
     * Gets an element's number or position in the current set of controls.
     * Maps to native aria-posinset attribute. It starts from 1 of type number
     */
    public get AriaPositionInSet(): number {
        return this.ariaPositionInSet;
    }

    /**
     * Sets the number of items in the current set of controls. Maps to native aria-setsize attribute of type number
     */
    public set AriaSetSize(ariaSetSize: number){
        this.ariaSetSize = ariaSetSize;
    }

    /**
     * Gets the number of items in the current set of controls. Maps to native aria-setsize attribute of type number
     */
    public get AriaSetSize(): number {
        return this.ariaSetSize;
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
     * Sets the error message of type string
     */
    public set ErrorMessage(errorMessage: string){
        this.errorMessage = errorMessage;
    }

    /**
     * Gets the error message of type string
     */
    public get ErrorMessage(): string {
        return this.errorMessage;
    }

    /**
     * Sets the key of the initially selected option of type string
     */
    public set SelectedKey(selectedKey: string){
        this.selectedKey = selectedKey;
    }

    /**
     * Gets the key of the initially selected option of type string
     */
    public get SelectedKey(): string {
        return this.selectedKey;
    }

    /**
     * Sets the collection of options for this Dropdown of type [PropertyPaneDropDownOption]
     */
    public set Options(options: [PropertyPaneDropDownOption]){
        this.options = options;
    }

    /**
     * Gets the collection of options for this Dropdown of type [PropertyPaneDropDownOption]
     */
    public get Options(): [PropertyPaneDropDownOption] {
        return this.options;
    }
}