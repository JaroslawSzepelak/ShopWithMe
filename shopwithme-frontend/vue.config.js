/* eslint-disable prettier/prettier */
const path = require("path");

module.exports = {
    css: {
        loaderOptions: {
            sass: {
                additionalData: `@import "@/assets/variables.scss";`,
            },
        },
    },
};