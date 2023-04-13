// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

import { IPropertyPaneFieldProperties } from './IPropertyPaneFieldProperties';

/**
 * Sharepoint PropertyPaneTextFieldProperties object
 */
export class PropertyPaneTextFieldProperties implements IPropertyPaneFieldProperties{
    private label: string;
    private value: string;
    private ariaLabel: string;
    private deferredValidationTime: number;
    private description: string;
    private disabled: boolean;
    private errorMessage: string;
    private logName: string;
    private maxLength: number;
    private multiline: boolean;
    private placeholder: string;
    private resizable: boolean;
    private rows: number;
    private underlined: boolean;
    private validateOnFocusIn: boolean;
    private validateOnFocusOut: boolean;

    /**
     * Initializes a new instance of the PropertyPaneTextFieldProperties class
     */
    public PropertyPaneTextFieldProperties(){
        // Do nothing
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
     * Sets the value of type string
     */
    public set Value(value: string){
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
     * Sets the amount of time to wait before validating after the users stop typing in ms of type number
     */
    public set DeferredValidationTime(deferredValidationTime: number){
        this.deferredValidationTime = deferredValidationTime;
    }

    /**
     * Gets the amount of time to wait before validating after the users stop typing in ms of type number
     */
    public get DeferredValidationTime(): number {
        return this.deferredValidationTime;
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
     * Sets the name used to log PropertyPaneTextField value changes for engagement tracking of type string
     */
    public set LogName(logName: string){
        this.logName = logName;
    }

    /**
     * Gets the name used to log PropertyPaneTextField value changes for engagement tracking of type string
     */
    public get LogName(): string {
        return this.logName;
    }

    /**
     * Sets the maximum number of characters that the PropertyPaneTextField can have of type number
     */
    public set MaxLength(maxLength: number){
        this.maxLength = maxLength;
    }

    /**
     * Gets the maximum number of characters that the PropertyPaneTextField can have of type number
     */
    public get MaxLength(): number {
        return this.maxLength;
    }

    /**
     * Sets a value indicating whether or not the text field is a multiline text field of type boolean
     */
    public set Multiline(multiline: boolean){
        this.multiline = multiline;
    }

    /**
     * Gets a value indicating whether or not the text field is a multiline text field of type boolean
     */
    public get Multiline(): boolean {
        return this.multiline;
    }

    /**
     * Sets the placeholder text to be displayed in the text field of type string
     */
    public set Placeholder(placeholder: string){
        this.placeholder = placeholder;
    }

    /**
     * Gets the placeholder text to be displayed in the text field of type string
     */
    public get Placeholder(): string {
        return this.placeholder;
    }

    /**
     * Sets a value indicating whether or not the multiline text field is resizable of type boolean
     */
    public set Resizable(resizable: boolean){
        this.resizable = resizable;
    }

    /**
     * Gets a value indicating whether or not the multiline text field is resizable of type boolean
     */
    public get Resizable(): boolean {
        return this.resizable;
    }

    /**
     * Sets the value that specifies the visible height of a text area(multiline text TextField), 
     * in lines.maximum number of characters that the PropertyPaneTextField can have of type number
     */
    public set Rows(rows: number){
        this.rows = rows;
    }

    /**
     * Gets the value that specifies the visible height of a text area(multiline text TextField), 
     * in lines.maximum number of characters that the PropertyPaneTextField can have of type number
     */
    public get Rows(): number {
        return this.rows;
    }

    /**
     * Sets a value indicating whether or not the text field is underlined of type boolean
     */
    public set Underlined(underlined: boolean){
        this.underlined = underlined;
    }

    /**
     * Gets a value indicating whether or not the text field is underlined of type boolean 
     */
    public get Underlined(): boolean {
        return this.underlined;
    }

    /**
     * Sets a value indicating whether to run validation when the 
     * PropertyPaneTextField is focused of type boolean
     */
    public set ValidateOnFocusIn(validateOnFocusIn: boolean){
        this.validateOnFocusIn = validateOnFocusIn;
    }

    /**
     * Gets a value indicating whether to run validation when the 
     * PropertyPaneTextField is focused of type boolean
     */
    public get ValidateOnFocusIn(): boolean {
        return this.validateOnFocusIn;
    }

    /**
     * Sets a value indicating whether to run validation when the 
     * PropertyPaneTextField is out of focus or on blur of type boolean
     */
    public set ValidateOnFocusOut(validateOnFocusOut: boolean){
        this.validateOnFocusOut = validateOnFocusOut;
    }

    /**
     * Gets a value indicating whether to run validation when the 
     * PropertyPaneTextField is out of focus or on blur of type boolean
     */
    public get ValidateOnFocusOut(): boolean {
        return this.validateOnFocusOut;
    }
}