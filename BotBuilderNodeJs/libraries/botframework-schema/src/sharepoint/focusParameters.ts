// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

/**
 * Sharepoint FocusParameters object
 */
export class FocusParameters {
    private focusTarget: string;
    private ariaLive: FocusParameters.AriaLive;
    
    /**
     * Initializes a new instance of the FocusParameters class
     */
    public FocusParameters() {
        // Do nothing
    }

    /**
     * Sets focus target property of type string
     */
    public set FocusTarget(focusTarget: string) {
        this.focusTarget = focusTarget;
    }

    /**
     * Gets focus target property of type string
     */
    public get FocusTarget(): string {
        return this.focusTarget;
    }

    /**
     * Sets isTeamsDeepLink property of type boolean
     */
    public set AriaLive (ariaLive: FocusParameters.AriaLive) {
        this.ariaLive = ariaLive;
    }

    /**
     * Gets isTeamsDeepLink property of type boolean
     */
    public get AriaLive(): FocusParameters.AriaLive {
        return this.ariaLive; 
    }
}

export namespace FocusParameters
{
    export enum AriaLive {
        Polite = "polite",
        Assertive = "assertive",
        Off = "off"
    }
}