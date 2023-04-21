// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { BaseCardParameters } from './baseCardParameters';
import { ICardParameters } from './ICardParameters';

/**
 * Sharepoint sign in card view parameters
 */
export class SignInCardParameters extends BaseCardParameters implements ICardParameters {
    private primaryText: string;
    private description: string;
    private signInButtonText: string;
    private uri: string;
    private connectionName: string;

    /**
     * Initializes a new instance of the SignInCardParameters class
     */
    public SignInCardParameters(){
        // Do nothing
    }

    /**
     * Sets primary text property of type string
     */
    public set PrimaryText(primaryText: string){
        this.primaryText = primaryText;
    }

    /**
     * Gets primary text property of type string
     */
    public get PrimaryText(): string {
        return this.primaryText;
    }

    /**
     * Sets description property of type string
     */
    public set Description(description: string){
        this.description = description;
    }

    /**
     * Gets description property of type string
     */
    public get Description(): string {
        return this.description;
    }

    /**
     * Sets sign in button text property of type string
     */
    public set SignInButtonText(signInButtonText: string){
        this.signInButtonText = signInButtonText;
    }

    /**
     * Gets sign in button text property of type string
     */
    public get SignInButtonText(): string {
        return this.signInButtonText;
    }

    /**
     * Sets uri property of type string
     */
    public set Uri(uri: string){
        this.uri = uri;
    }

    /**
     * Gets uri property of type string
     */
    public get Uri(): string {
        return this.uri;
    }

    /**
     * Sets connection name property of type string
     */
    public set ConnectionName(connectionName: string){
        this.connectionName = connectionName;
    }

    /**
     * Gets connection name property of type string
     */
    public get ConnectionName(): string {
        return this.connectionName;
    }
}