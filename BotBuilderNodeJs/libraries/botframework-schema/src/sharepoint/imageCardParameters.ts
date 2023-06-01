// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
import { BaseCardParameters } from './baseCardParameters';

/**
 * Sharepoint image card view parameters
 */
export class ImageCardParameters extends BaseCardParameters {
    private primaryText: string;
    private imageUrl: string;
    private imageAltText: string;

    /**
     * Initializes a new instance of the ImageCardParameters class
     */
    public ImageCardParameters() {
        // Do nothing
    }

    /**
     * Sets title property of type string
     */
    public set PrimaryText(primaryText: string) {
        this.primaryText = primaryText;
    }

    /**
     * Gets title property of type string
     */
    public get PrimaryText(): string {
        return this.primaryText;
    }

    /**
     * Sets image url property of type string
     */
    public set ImageUrl(imageUrl: string) {
        this.imageUrl = imageUrl;
    }

    /**
     * Gets image url property of type string
     */
    public get ImageUrl(): string {
        return this.imageUrl;
    }

    /**
     * Sets image alt text property of type string
     */
    public set ImageAltText(imageAltText: string) {
        this.imageAltText = imageAltText;
    }

    /**
     * Gets image alt text property of type string
     */
    public get ImageAltText(): string {
        return this.imageAltText;
    }
}