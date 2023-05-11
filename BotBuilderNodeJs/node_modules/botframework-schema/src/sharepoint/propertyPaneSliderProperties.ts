// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';

/**
 * Sharepoint PropertyPaneSliderProperties object
 */
export class PropertyPaneSliderProperties implements IPropertyPaneFieldProperties {
    private label: string;
    private value: string;
    private ariaLabel: string;
    private disabled: boolean;
    private max: number;
    private min: number;
    private step: number;
    private showValue: boolean;
    
    /**
     * Initializes a new instance of the PropertyPaneSliderProperties class
     */
    public PropertyPaneSliderProperties() {
        this.step = 1;
    }

    /**
     * Sets the label of type string
     */
    public set Label(label: string) {
        this.label = label;
    }

    /**
     * Gets the label of type string
     */
    public get Label(): string {
        return this.label;
    }

    /**
     * Sets the value of type string
     */
    public set Value(value: string) {
        this.value = value;
    }

    /**
     * Gets the value of type string
     */
    public get Value(): string {
        return this.value;
    }


    /**
     * Sets the aria label of type string
     */
    public set AriaLabel(ariaLabel: string) {
        this.ariaLabel = ariaLabel;
    }

    /**
     * Gets the aria label of type string
     */
    public get AriaLabel(): string {
        return this.ariaLabel;
    }


    /**
     * Sets a value indicating whether this control is enabled or not of type boolean
     */
     public set Disabled(disabled: boolean) {
        this.disabled = disabled;
    }

    /**
     * Gets a value indicating whether this control is enabled or not of type boolean
     */
    public get Disabled(): boolean {
        return this.disabled;
    }

    /**
     * Sets the max value of the Slider of type number
     */
    public set Max(max: number) {
        this.max = max;
    }

    /**
     * Gets the max value of the Slider of type number
     */
    public get Max(): number {
        return this.max;
    }

    /**
     * Sets the min value of the Slider of type number
     */
    public set Min(min: number) {
        this.min = min;
    }

    /**
     * Gets the min value of the Slider of type number
     */
    public get Min(): number {
        return this.min;
    }

    /**
     * Sets the difference between the two adjacent values of the Slider. Defaults to 1. of type number
     */
    public set Step(step: number) {
        this.step = step;
    }

    /**
     * Gets the difference between the two adjacent values of the Slider. Defaults to 1. of type number
     */
    public get Step(): number {
        return this.step;
    }

    /**
     * Sets a value indicating whether to show the value on the right of the Slider of type boolean
     */
    public set ShowValue(showValue: boolean) {
        this.showValue = showValue;
    }

    /**
     * Gets a value indicating whether to show the value on the right of the Slider of type boolean
     */
    public get ShowValue(): boolean {
        return this.showValue;
    }
}